namespace XOGame.Core
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int SecretToken  { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }

        public void Serialize(ByteWriter writer, bool full)
        {
            writer.Write(Username);
            writer.Write(PositionX);
            writer.Write(PositionY);
            if (full)
            {
                writer.Write(Password);
                writer.Write(SecretToken);
            }
        }

        public void Deserialize(ByteReader reader,bool full)
        {
            Username = reader.ReadString();
            PositionX = reader.ReadInt();
            PositionY = reader.ReadInt();
            if (full)
            {
                Password = reader.ReadString();
                SecretToken = reader.ReadInt();
            }
        }

        
    }
}