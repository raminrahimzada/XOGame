using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using XOGame.Client;
using XOGame.Core;

namespace XOGame.Launcher
{
    public partial class GameForm : Form
    {
        private readonly List<UserModel> _users;
        private   int _positionX;
        private   int _positionY;
        private bool _gameUpdated;

        private const int CircleSize = 50;


        public GameForm()
        {
            InitializeComponent();
            _users=new List<UserModel>();
            Paint += GameForm_Paint;
            KeyPreview = true;
            KeyDown += GameForm_KeyDown;
            MouseMove += GameForm_MouseMove;
            Load += GameForm_Load;
            Click += GameForm_Click;
            App.PacketReceivedEvent += App_PacketReceivedEvent;
        }

        //public override string Text => "User : " + ClientStorage.Instance.Username;

        private void GameForm_Load(object sender, EventArgs e)
        {
            this.Text = "User : " + ClientStorage.Instance.Username;
            new Thread(SendStateToServer)
            {
                IsBackground = true
            }.Start();
        }

        private void GameForm_Click(object sender, EventArgs e)
        {
            //this.Invalidate();
        }

        private void GameForm_MouseMove(object sender, MouseEventArgs e)
        {
            //_positionX = e.X;
            //_positionY = e.Y;
            //this.Invalidate();
        }

        

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            const int speed = 10;
            if(e.KeyCode==Keys.D|| e.KeyCode == Keys.Right)
            {
                _positionX += speed;
                _gameUpdated = true;
            }
            if (e.KeyCode == Keys.A|| e.KeyCode == Keys.Left)
            {
                _positionX -= speed;
                _gameUpdated = true;
            }
            if (e.KeyCode == Keys.W|| e.KeyCode == Keys.Up)
            {
                _positionY -= speed;
                _gameUpdated = true;
            }
            if (e.KeyCode == Keys.S|| e.KeyCode == Keys.Down)
            {
                _positionY += speed;
                _gameUpdated = true;
            }

            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
            
            App.PacketReceivedEvent -= App_PacketReceivedEvent;
        }
        private void App_PacketReceivedEvent(GamePacket packet)
        {
            bool renderAgain = false;
            switch (packet)
            {
                case UserStatePacket usersListPacket:
                    renderAgain=UpdateUsersPositions(ref usersListPacket);                    
                    break;
            }

            if (renderAgain && IsHandleCreated)
                BeginInvoke(new MethodInvoker(Invalidate));
        }

        public async void SendStateToServer()
        {
            while (true)
            {
                Thread.Yield();
                //Thread.Sleep(TimeSpan.FromMilliseconds(10));
                if (_gameUpdated)
                {
                    _gameUpdated = false;
                    await App.Client.Send(new SetStatePacket(App.Storage.SecretToken, _positionX, _positionY));
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(new SolidBrush(Color.Green));
            foreach (var user in _users)
            {
                Rectangle rect = new Rectangle(user.PositionX, user.PositionY, CircleSize, CircleSize);
                e.Graphics.DrawArc(pen, rect, 0, 360);
                Font font=Font;
                PointF point = new PointF(user.PositionX, user.PositionY);
                e.Graphics.DrawString(user.Username,font,pen.Brush,point);
            }
        }

        private bool UpdateUsersPositions(ref UserStatePacket packet)
        {
            bool changed = false;
            //foreach (var u in packet.Users)
            var u = packet.User;
            {
                var user = _users.FirstOrDefault(x => x.Username == u.Username);
                if (user != null)
                {
                    if (user.PositionX != u.PositionX) changed = true;
                    if (user.PositionY != u.PositionY) changed = true;

                    user.PositionX = u.PositionX;
                    user.PositionY = u.PositionY;
                }
                else
                {
                    changed = true;
                    _users.Add(u);
                }
            }

            return changed;
        }
        

        
    }
}
