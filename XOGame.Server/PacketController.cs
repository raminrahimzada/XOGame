using System;
using System.Collections.Generic;
using XOGame.Core;

namespace XOGame.Server
{
    public class PacketController
    {
        private readonly UserService _userService;

        public PacketController(UserService userService)
        {
            _userService = userService;
        }

        public IEnumerable<ServerPacket> Handle(ClientPacket packet)
        {
            if (packet == null) throw new Exception("Null Packet");

            switch (packet)
            {
                case LoginPacket loginPacket:
                    return Handle(loginPacket);
                
                case SetStatePacket statePacket:
                    return Handle(statePacket);
                
                case HeartbeatPacket heartbeatPacket:
                    return Handle(heartbeatPacket);
                //server do not need other packets
                default:
                    return new ServerPacket[0];
            }
        }


        #region Specific Handlers

        private IEnumerable<ServerPacket> Handle(HeartbeatPacket packet)
        {
            yield return new HeartbeatResponsePacket(DateTime.Now);
        }

        private   IEnumerable<ServerPacket> Handle(SetStatePacket packet)
        {
            var user = _userService.Find(packet.SecretToken);
            if (user != null)
            {
                user.PositionX = packet.PositionX;
                user.PositionY = packet.PositionY;
                yield return new UserStatePacket(ref user);
            }
        }
        private IEnumerable<ServerPacket> Handle(LoginPacket packet)
        {
            var result = _userService.Login(packet.Username, packet.Password);
            var resultPacket = new LoginResponsePacket(result);
            yield return resultPacket;
            //if login success
            if (result != 0)
            {
                var user = _userService.Find(resultPacket.SecretToken);
                yield return new UserStatePacket(ref user);
            }
        }
        #endregion
    }
}