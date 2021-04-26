using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using XOGame.Core;

namespace XOGame.Client
{
    public class GameClient:IDisposable
    {
        private readonly UdpClient _client;
        private bool _exit;
        public GameClient()
        {
            string ip = GameSettings.serverIp;
            int port = GameSettings.serverPort;
            var server = new IPEndPoint(IPAddress.Parse(ip), port);
            _client=new UdpClient();
            _client.Connect(server);
            new Thread(ListenThread).Start();
        }

        public GamePacketHandlerDelegate<byte[]> ReceiveHandler { get; set; }

        private  async void ListenThread(object obj)
        {
            while (!_exit)
            {
                var result = await _client.ReceiveAsync();
                byte[] buffer = result.Buffer;
                ReceiveHandler?.Invoke(ref buffer);
            }
        }

        public async Task Send<T>(T packet) where T : GamePacket
        {
            var buffer = packet.Serialize();
            await _client.SendAsync(buffer, buffer.Length);
        }

        public void Dispose()
        {
            _exit = true;
            _client?.Dispose();
        }
    }
}