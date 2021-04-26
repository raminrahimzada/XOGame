using System;
using XOGame.Core;

namespace XOGame.Client
{
    public class PacketController:Core.PacketDetector
    {
        private readonly ClientStorage _clientStorage;

        public PacketController(ClientStorage clientStorage)
        {
            _clientStorage = clientStorage;
        }
       
        public GamePacket Handle(ref byte[] buffer)
        {
            var packet = Detect(ref buffer,out var packetType);
            if (packetType == 0) return null;
            if (packet == null) throw new Exception("Null Packet");
            Handle(ref packet);
            return packet;
        }
        public void Handle(ref GamePacket packet)
        {
            switch (packet)
            {
                case LoginResponsePacket authPacket:
                    Handle(ref authPacket);
                    break;
                case UserStatePacket usersListPacket:
                    Handle( ref usersListPacket);
                    break;

                //client do not need other packets
                default:
                    return;
            }
        }

       
        private void Handle(ref UserStatePacket packet)
        {
            var user = packet.User;
            Console.WriteLine($"got user {user.Username} is in ({user.PositionX},{user.PositionY})");
        }
        private void Handle(ref LoginResponsePacket packet)
        {
            if (packet.SecretToken == 0)
            {
                Console.WriteLine("Invalid Password");
            }
            else
            {
                Console.WriteLine("Auth Completed ,Secret token = " + packet.SecretToken);

                _clientStorage.SecretToken = packet.SecretToken;
            }
        }
       
    }
}