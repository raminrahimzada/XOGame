using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XOGame.Client;
using XOGame.Core;

namespace XOGame.Launcher
{
    public class LoginForm : Form
    {
        private TextBox _textBoxUsername;
        private Button _buttonLogin;
        private Label _labelUsername;
        private Label _labelPassword;
        private Label _labelError;
        private Label labelHeartBeat;
        private TextBox _textBoxPassword;

        private void InitializeComponent()
        {
            this._textBoxUsername = new System.Windows.Forms.TextBox();
            this._buttonLogin = new System.Windows.Forms.Button();
            this._labelUsername = new System.Windows.Forms.Label();
            this._labelPassword = new System.Windows.Forms.Label();
            this._textBoxPassword = new System.Windows.Forms.TextBox();
            this._labelError = new System.Windows.Forms.Label();
            this.labelHeartBeat = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _textBoxUsername
            // 
            this._textBoxUsername.Location = new System.Drawing.Point(193, 21);
            this._textBoxUsername.Name = "_textBoxUsername";
            this._textBoxUsername.Size = new System.Drawing.Size(100, 27);
            this._textBoxUsername.TabIndex = 0;
            // 
            // _buttonLogin
            // 
            this._buttonLogin.Location = new System.Drawing.Point(157, 109);
            this._buttonLogin.Name = "_buttonLogin";
            this._buttonLogin.Size = new System.Drawing.Size(136, 29);
            this._buttonLogin.TabIndex = 2;
            this._buttonLogin.Text = "Login or Register";
            this._buttonLogin.UseVisualStyleBackColor = true;
            this._buttonLogin.Click += new System.EventHandler(this._buttonLogin_Click);
            // 
            // _labelUsername
            // 
            this._labelUsername.AutoSize = true;
            this._labelUsername.Location = new System.Drawing.Point(111, 24);
            this._labelUsername.Name = "_labelUsername";
            this._labelUsername.Size = new System.Drawing.Size(75, 20);
            this._labelUsername.TabIndex = 2;
            this._labelUsername.Text = "Username";
            // 
            // _labelPassword
            // 
            this._labelPassword.AutoSize = true;
            this._labelPassword.Location = new System.Drawing.Point(111, 60);
            this._labelPassword.Name = "_labelPassword";
            this._labelPassword.Size = new System.Drawing.Size(70, 20);
            this._labelPassword.TabIndex = 2;
            this._labelPassword.Text = "Password";
            // 
            // _textBoxPassword
            // 
            this._textBoxPassword.Location = new System.Drawing.Point(193, 57);
            this._textBoxPassword.Name = "_textBoxPassword";
            this._textBoxPassword.Size = new System.Drawing.Size(100, 27);
            this._textBoxPassword.TabIndex = 1;
            // 
            // _labelError
            // 
            this._labelError.AutoSize = true;
            this._labelError.Location = new System.Drawing.Point(12, 151);
            this._labelError.Name = "_labelError";
            this._labelError.Size = new System.Drawing.Size(39, 20);
            this._labelError.TabIndex = 2;
            this._labelError.Text = "..........";
            // 
            // labelHeartBeat
            // 
            this.labelHeartBeat.AutoSize = true;
            this.labelHeartBeat.Location = new System.Drawing.Point(12, 177);
            this.labelHeartBeat.Name = "labelHeartBeat";
            this.labelHeartBeat.Size = new System.Drawing.Size(39, 20);
            this.labelHeartBeat.TabIndex = 3;
            this.labelHeartBeat.Text = "..........";
            // 
            // LoginForm
            // 
            this.AcceptButton = this._buttonLogin;
            this.ClientSize = new System.Drawing.Size(461, 204);
            this.Controls.Add(this.labelHeartBeat);
            this.Controls.Add(this._labelError);
            this.Controls.Add(this._labelPassword);
            this.Controls.Add(this._labelUsername);
            this.Controls.Add(this._buttonLogin);
            this.Controls.Add(this._textBoxPassword);
            this.Controls.Add(this._textBoxUsername);
            this.DoubleBuffered = true;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //private readonly GameForm  _nextForm = new GameForm();
        public LoginForm()
        {
            InitializeComponent();
            App.PacketReceivedEvent += App_PacketReceivedEvent;
            //textBoxServerIp.Text = Dns.GetHostAddresses("localhost").FirstOrDefault()?.ToString();
            new Thread(HeartBeat) {IsBackground = true}.Start();
        }

        private async void HeartBeat()
        {
            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                await App.Client.Send(new HeartbeatPacket(App.Storage.SecretToken));
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            App.PacketReceivedEvent -= App_PacketReceivedEvent;
        }

        private void App_PacketReceivedEvent(GamePacket packet)
        {
            switch (packet)
            {
                case HeartbeatResponsePacket heartbeatResponsePacket:
                    labelHeartBeat.Text = "Server Time : " + heartbeatResponsePacket.ServerTime.ToString("G");
                    break;
                case LoginResponsePacket authPacket:
                    if (authPacket.SecretToken == 0)
                    {
                        MessageBox.Show("Invalid Username or Password");
                    }
                    else
                    {
                        ClientStorage.Instance.SecretToken = authPacket.SecretToken;
                        this.Invoke(new MethodInvoker(() =>
                        {
                            new GameForm().Show();
                        }));
                        //this.Hide();
                    }
                    break;
            }
        }

        private async void _buttonLogin_Click(object sender, System.EventArgs e)
        {
            var username = _textBoxUsername.Text;
            var password = _textBoxPassword.Text;
            var packet = new LoginPacket(username, password);
            await App.Client.Send(packet);
            ClientStorage.Instance.Username = username;
        }
    }
}