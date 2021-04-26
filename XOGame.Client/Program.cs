using System;
using System.Threading.Tasks;
using XOGame.Core;

namespace XOGame.Client
{
    public class GameSettings
    {
        public static string serverIp = "192.168.31.166";
        public static int serverPort = 11000;
    }

    public static class App
    {
        public static GameClient Client;
        public static ClientStorage Storage;
        public static event Action<GamePacket> PacketReceivedEvent;
        public static PacketController Controller;
        public static void Setup()
        {
            Client=new GameClient();
            Storage = ClientStorage.Instance;
            Controller = new PacketController(Storage);
            Client.ReceiveHandler = ReceiveHandler;
        }

        private static GamePacket ReceiveHandler(ref byte[] buffer)
        {
            var packet = Controller.Handle(ref buffer);
            PacketReceivedEvent?.Invoke(packet);
            return packet;
        }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
           App.Setup();
           
            int i = 1;
            while (true)
            {
                Console.WriteLine("Enter username :");
                var username = Console.ReadLine() ?? "";

                Console.WriteLine("Enter password :");
                var password = Console.ReadLine() ?? "";
                var packet = new LoginPacket(username, password);
                await App.Client.Send(packet);
                await App.Client.Send(new SetStatePacket(App.Storage.SecretToken, 100 - i, 150 + i++));
            }
        }

        
    }
}
