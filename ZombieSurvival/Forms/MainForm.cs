using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinFormsGameSDK;
using WinFormsGameSDK.Drawing;
using WinFormsGameSDK.Forms;
using ZombieSurvival.Sprites;

namespace ZombieSurvival.Forms
{
    public partial class MainForm : GameForm
    {
        private readonly GameSession gameSession = new GameSession();
        private readonly MainFormRenderer renderer = new MainFormRenderer();

        /// <summary>
        /// Gets or sets whether to show debug information.
        /// </summary>
        public override bool ShowDebugInfo
        {
            get { return base.ShowDebugInfo; }
            set
            {
                base.ShowDebugInfo = value;
                renderer.ShowDebugMarkings = value;
            }
        }

        public MainForm()
        {
            InitializeComponent();
#if DEBUG
            ShowDebugInfo = true;
#endif
            BindKeys();
        }

        private void BindKeys()
        {
            // Toggle full-screen.
            KeyActionBindings.Add((keyDown) =>
            {
                if (keyDown)
                    FullScreen = !FullScreen;
            }, Keys.F11);
            // Exit full-screen or application.
            KeyActionBindings.Add((keyDown) =>
            {
                if (keyDown)
                {
                    if (FullScreen)
                    {
                        FullScreen = false;
                    }
                    else Close();
                }
            },
            Keys.Escape);
            // Move forwards.
            KeyActionBindings.Add(keyDown =>
            {
                if (keyDown)
                    gameSession.MoveDirection |= MoveDirection.Forwards;
                else
                    gameSession.MoveDirection -= MoveDirection.Forwards;

            }, Keys.W);
            // Move backwards.
            KeyActionBindings.Add(keyDown =>
            {
                if (keyDown)
                    gameSession.MoveDirection |= MoveDirection.Backwards;
                else
                    gameSession.MoveDirection -= MoveDirection.Backwards;

            }, Keys.S);
            // Move left.
            KeyActionBindings.Add(keyDown =>
            {
                if (keyDown)
                    gameSession.MoveDirection |= MoveDirection.Left;
                else
                    gameSession.MoveDirection -= MoveDirection.Left;

            }, Keys.A);
            // Move right.
            KeyActionBindings.Add(keyDown =>
            {
                if (keyDown)
                    gameSession.MoveDirection |= MoveDirection.Right;
                else
                    gameSession.MoveDirection -= MoveDirection.Right;

            }, Keys.D);
            // Use.
            KeyActionBindings.Add(keyDown =>
            {
                if (keyDown)
                    gameSession.Use();

            }, Keys.E);
            // Toggle zombie spawner.
            KeyActionBindings.Add(keyDown =>
            {
                if (keyDown)
                    gameSession.ZombieSpawnerEnabled = !gameSession.ZombieSpawnerEnabled;
            }, Keys.Z);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    gameSession.Firing = true;
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            switch (e.Button)
            {
                case MouseButtons.Left:
                    gameSession.Firing = false;
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var angle = e.Location.AngleTo(ToClientPoint(gameSession.LocalPlayer.Position));
            gameSession.LocalPlayer.Rotate(angle);
        }

        private string GetDebugInfo()
        {
            StringBuilder SB = new StringBuilder();
            SB.AppendLine("FPS: " + FrameRate);
            SB.AppendLine("Zombie spawner enabled: " + gameSession.ZombieSpawnerEnabled);
            SB.AppendLine("Player position: " + gameSession.LocalPlayer.Position);
            SB.AppendLine("Tick rate: " + GameSessionBase.TickRate);
            SB.AppendLine("Sprite count: " + gameSession.SpriteMan.SpriteCount);
            SB.AppendLine("Player Facing Degree: " + (int)(gameSession.LocalPlayer.FacingDegree + 0.5));
            return SB.ToString();
        }

        protected override void Render(Graphics graphics)
        {
            DebugCaption = GetDebugInfo();

            foreach (var contrail in gameSession.SpriteMan.Contrails)
            {
                renderer.DrawContrail(graphics, ToClientLine(contrail.Line), contrail.Kind);
            }

            foreach (var vehicle in gameSession.SpriteMan.Vehicles)
                renderer.DrawTruck(graphics, ToClientPoints(vehicle.Model.Points));

            foreach (var player in gameSession.SpriteMan.Players)
            {
                DrawBiped(graphics, player);
                renderer.DrawPistol(graphics, ToClientLine(player.Gun.Line));
            }

            foreach (var zombie in gameSession.SpriteMan.Zombies)
                DrawBiped(graphics, zombie);

            foreach (var blocker in gameSession.SpriteMan.Blockers)
                renderer.DrawBlocker(graphics, ToClientPoints(blocker.MovementCollision.Points));

            if (gameSession.LocalPlayer.NearbyVehicle != null)
            {
                if (!gameSession.LocalPlayer.IsDriving)
                {
                    string message = "Enter vehicle: " + gameSession.LocalPlayer.ID;
                    PointF driverPos = ToClientPoint(gameSession.LocalPlayer.NearbyVehicle.GetDriverPosition());
                    renderer.DrawEnterVehicleText(graphics, message, driverPos);
                }
            }

            PointF pos = PointF.Empty;
            PointF pos2 = ToClientPoint(pos);
            PointF offset = new PointF(pos.X + pos2.X, pos.Y + pos2.Y);
            renderer.DrawDebugMarkings(graphics, gameSession.SpriteMan.All, offset);
        }

        private void DrawBiped(Graphics graphics, BipedSprite biped)
        {
            Line[] figure = ToClientLines(biped.GetFigure());
            renderer.DrawBiped(graphics, ToClientBounds(biped.HeadBounds), figure);
        }

        /// <summary>
        /// Converts the specified game points to client point of this Form.
        /// </summary>
        private PointF[] ToClientPoints(PointF[] points)
        {
            PointF[] newPoints = new PointF[points.Length];

            for (int i = 0; i < points.Length; i++)
                newPoints[i] = ToClientPoint(points[i]);

            return newPoints;
        }

        /// <summary>
        /// Converts the specified game rectangle to a client rectangle of this Form.
        /// </summary>
        private RectangleF ToClientBounds(RectangleF gameBounds)
        {
            return new RectangleF(ToClientPoint(gameBounds.Location), gameBounds.Size);
        }

        /// <summary>
        /// Converts the specified game line to a client line of this Form.
        /// </summary>
        private Line ToClientLine(Line gameLine)
        {
            return new Line(ToClientPoint(gameLine.Start), ToClientPoint(gameLine.End));
        }

        /// <summary>
        /// Converts the specified game lines to client lines of this Form.
        /// </summary>
        private Line[] ToClientLines(Line[] gameLines)
        {
            Line[] newLines = new Line[gameLines.Length];

            for (int i = 0; i < gameLines.Length; i++)
                newLines[i] = ToClientLine(gameLines[i]);

            return newLines;
        }

        /// <summary>
        /// Converts the specified game point to a client point of this Form.
        /// </summary>
        private PointF ToClientPoint(Point gamePoint)
        {
            var centerForm = ClientRectangle.GetCenter();
            var playerPos = gameSession.LocalPlayer.Position;
            float xOffset = centerForm.X - playerPos.X;
            float yOffset = centerForm.Y - playerPos.Y;
            return new PointF(gamePoint.X + xOffset, gamePoint.Y + yOffset);
        }

        /// <summary>
        /// Converts the specified game point to a client point of this Form.
        /// </summary>
        private PointF ToClientPoint(PointF gamePoint)
        {
            var centerForm = ClientRectangle.GetCenter();
            var playerPos = gameSession.LocalPlayer.Position;
            float xOffset = centerForm.X - playerPos.X;
            float yOffset = centerForm.Y - playerPos.Y;
            return new PointF(gamePoint.X + xOffset, gamePoint.Y + yOffset);
        }
    }
}
