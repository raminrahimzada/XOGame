namespace XOGame.Core
{
    public enum PacketTypes : byte
    {
        LoginRequest = 1,
        LoginResponse=2,
        UserStateResponse=3,
        SetStateRequest=4,
        HeartBeat=5,
        HeartBeatResponse=6
    }
}