using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public GameForm()
        {
            InitializeComponent();
            _users=new List<UserModel>();
            this.Paint += GameForm_Paint;
            this.KeyPreview = true;
            this.KeyDown += GameForm_KeyDown;
            this.MouseMove += GameForm_MouseMove;
            this.Load += GameForm_Load;
            this.Click += GameForm_Click;
            App.PacketReceivedEvent += App_PacketReceivedEvent;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
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

        private bool _gameUpdated = false;

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

            this.Invalidate();
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
                case UserStatesPacket usersListPacket:
                    renderAgain=UpdateUsersPositions(ref usersListPacket);                    
                    break;
            }

            if (renderAgain && this.IsHandleCreated)
                this.BeginInvoke(new MethodInvoker(this.Invalidate));
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
                    await App.Client.Send(new StatePacket(App.Storage.SecretToken, _positionX, _positionY));
                }
            }
        }

        private const int circleSize = 50;
        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(new SolidBrush(Color.Green));
            foreach (var user in _users)
            {
                Rectangle rect = new Rectangle(user.PositionX, user.PositionY, circleSize, circleSize);
                e.Graphics.DrawArc(pen, rect, 0, 360);
                Font font=this.Font;
                PointF point = new PointF(user.PositionX, user.PositionY);
                e.Graphics.DrawString(user.Username,font,pen.Brush,point);
            }
        }
 

        private bool UpdateUsersPositions(ref UserStatesPacket packet)
        {
            bool changed = false;
            foreach (var u in packet.Users)
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
