using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using XOGame.Core;

namespace XOGame.Server
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            const int listenPort = 11000;
            var userService=new UserService();
            var controller = new PacketController(userService);
            
            var listener = new UdpClient(listenPort);
            int packetCounter = 0;
            long packetSizeCounter = 0;
            List<IPEndPoint> connectedEndpoints = new List<IPEndPoint>();

            //var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                Console.WriteLine("Waiting for clients...");

                while (true)
                {
                    var result = await listener.ReceiveAsync();
                    var buffer = result.Buffer;
                    ++packetCounter;
                    packetSizeCounter += buffer.Length;
                    var packet = PacketDetector.Detect(ref buffer, out var packetType);
                    if (packetType == 0)
                    {
                        //invalid packet go home dude
                    }
                    else
                    {
                        if (!connectedEndpoints.Contains(result.RemoteEndPoint))
                        {
                            connectedEndpoints.Add(result.RemoteEndPoint);
                        }

                        if (packet is ClientPacket clientPacket)
                        {
                            var outgoingPackets = controller.Handle(clientPacket);
                            foreach (var outgoingPacket in outgoingPackets)
                            {
                                buffer = outgoingPacket.Serialize();
                                if (outgoingPacket.IsBroadcast)
                                {
                                    foreach (var endPoint in connectedEndpoints)
                                    {
                                        await listener.SendAsync(buffer, buffer.Length, endPoint);
                                    }
                                }
                                else
                                {
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