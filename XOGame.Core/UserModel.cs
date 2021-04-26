namespace XOGame.Core
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int SecretToken  { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}