using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using XOGame.Core;

namespace XOGame.Server
{
    static class Extensions
    {
        public static async Task<UdpReceiveResult> ReceiveAsyncSafe(this UdpClient client)
        {
            try
            {
                return await client.ReceiveAsync();
            }
            catch
            {
                return new UdpReceiveResult();
            }
        }
    }
    class Program
    {
        private static async Task Main()
        {
            const int listenPort = 11000;
            var userService=new UserService();
            var controller = new PacketController(userService);
            
            var listener = new UdpClient(listenPort);
            int packetCounter = 0;
            long packetSizeCounter = 0;
            List<IPEndPoint> connectedEndpoints = new List<IPEndPoint>();

            try
            {
                Console.WriteLine("Waiting for clients...");
                while (true)
                {
                    var result = await listener.ReceiveAsyncSafe();
                    var buffer = result.Buffer;
                    if (buffer == null) continue;

                    ++packetCounter;
                    packetSizeCounter += buffer.Length;
                    var packet = PacketDetector.Detect(ref buffer, out var packetType);
                    if (packetType == 0|| packet==null)
                    {
                        //invalid packet go home dude
                    }
                    else
                    {
                        if (!connectedEndpoints.Contains(result.RemoteEndPoint))
                        {
                            Console.WriteLine("client connected:{0}", result.RemoteEndPoint);
                            connectedEndpoints.Add(result.RemoteEndPoint);
                        }

                        if (packet is ClientPacket clientPacket)
                        {
                            var outgoingPackets = controller.Handle(clientPacket);
                            ByteWriter writer=new ByteWriter();
                            foreach (var outgoingPacket in outgoingPackets)
                            {
                                writer.Reset();
                                outgoingPacket.SerializeRaw(ref writer);
                                buffer = writer.ToArray();
                                if (outgoingPacket.IsBroadcast)
                                {
                                    //if broadcast then send to all
                                    foreach (var endPoint in connectedEndpoints)
                                    {
                                        await listener.SendAsync(buffer, buffer.Length, endPoint);
                                    }
                                }
                                else
                                {
                                    //else direct to sender
                                    await listener.SendAsync(buffer, buffer.Length, result.RemoteEndPoint);
                                }
                            }
                        }
                    }

                    //var bufferString = Encoding.UTF8.GetString(buffer);
                    Console.Title =
                        $"received {packetCounter} packets ({packetSizeCounter / 1024.0 / 1024.0:N3}MB) from {connectedEndpoints.Count} endpoints";
                    //Console.WriteLine("{0} |> {1}", result.RemoteEndPoint, bufferString);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            listener.Close();
        }
    }
}