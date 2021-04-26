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
                
                case StatePacket statePacket:
                    return Handle(statePacket);

                //server do not need other packets
                default:
                    return new ServerPacket[0];
            }
        }


        #region Specific Handlers
        private   IEnumerable<ServerPacket> Handle(StatePacket packet)
        {
            _userService.UpdateState(packet);
            var users = _userService.GetAllUsers();
            yield return new UserStatesPacket(ref users);
        }
        private IEnumerable<ServerPacket> Handle(LoginPacket packet)
        {
            var result = _userService.Login(packet.Username, packet.Password);
            var resultPacket = new LoginResponsePacket(result);
            yield return resultPacket;
            //if login success
            if (result != 0)
            {
                var users = _userService.GetAllUsers();
                yield return new UserStatesPacket(ref users);
            }
        }
        

        #endregion
    }
}