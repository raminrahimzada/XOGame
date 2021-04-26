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
                case UserStatesPacket usersListPacket:
                    Handle( ref usersListPacket);
                    break;

                //client do not need other packets
                default:
                    return;
            }
        }

       
        private void Handle(ref UserStatesPacket packet)
        {
            Console.WriteLine("got users list:");
            foreach (var user in packet.Users)
            {
                Console.WriteLine($"{user.Username} is in ({user.PositionX},{user.PositionY})");
            }
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