namespace XOGame.Client
{
    public class ClientStorage
    {
        public int SecretToken { get; set; }
        public string Username { get; set; }


        private ClientStorage()
        {
            
        }
        public static ClientStorage Instance { get; }

        static ClientStorage()
        {
            Instance = new ClientStorage();
        }
    }
}