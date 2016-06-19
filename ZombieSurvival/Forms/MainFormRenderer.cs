using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using WinFormsGameSDK;
using WinFormsGameSDK.Drawing;
using WinFormsGameSDK.Sprites;
using ZombieSurvival.Sprites;

namespace ZombieSurvival.Forms
{
    /// <summary>
    /// Provides the rendering implementation for the <see cref="MainForm"/>.
    /// </summary>
    class MainFormRenderer : IDisposable
    {
        private readonly Font debugFont = new Font(new FontFamily("Segoe UI"), 12f);
        private readonly Font enterVehicleFont = new Font(new FontFamily("Segoe UI"), 16f);
        private readonly Pen bulletContrailPen = new Pen(Color.FromArgb(150, Color.Gray), 1f);
        private readonly Pen bloodContrailPen = new Pen(Color.DarkRed, 2f);
        private readonly Pen pistolPen = new Pen(Color.DarkGray, 6f);
        private readonly Pen debugPen = new Pen(Color.Lime, 1f);
        private readonly Pen bipedTorsoPen;
        private readonly Pen bipedArmsPen = new Pen(Color.White, 7f);

        /// <summary>
        /// Gets or sets whether to show debug markings, such as collision models.
        /// </summary>
        public bool ShowDebugMarkings { get; set; }

        public MainFormRenderer()
        {
            debugPen.CustomEndCap = new AdjustableArrowCap(15f, 15f, false);
            bipedTorsoPen = new Pen(Color.White, BipedSprite.ShoulderFatness / 2);
            bipedTorsoPen.EndCap = LineCap.Round;
            bipedTorsoPen.StartCap = LineCap.Round;
            bipedArmsPen.EndCap = LineCap.Round;
            bipedArmsPen.StartCap = LineCap.Round;
        }

        /// <summary>
        /// Draws vector normals, collision geometry, and sprite IDs.
        /// </summary>
        /// <param name="graphics">The surface to draw to.</param>
        /// <param name="sprites">The sprites to draw debug info for.</param>
        /// <param name="offset">The drawing offset so the game points can be offseted to client points.</param>
        public void DrawDebugMarkings(Graphics graphics, IEnumerable<Sprite> sprites, PointF offset)
        {
            if (!ShowDebugMarkings) return;

            foreach (var sprite in sprites)
            {
                // Draw normal.
                Vector2D endVector = sprite.Vector.Clone();
                endVector.Project(70f);
                graphics.DrawLine(debugPen, sprite.Position.Offset(offset), endVector.Position.Offset(offset));

                // Draw ID.
                graphics.DrawString(sprite.ID, debugFont, Brushes.Lime, sprite.Vector.Position.Offset(offset).Offset(50, 0));

                var collidable = sprite as CollidableSprite;

                if (collidable?.MovementCollision != null)
                {
                    var newPoints = new PointF[collidable.MovementCollision.Points.Length];
                    var oldPoints = collidable.MovementCollision.Points;

                    for (int i = 0; i < newPoints.Length; i++)
                    {
                        newPoints[i] = oldPoints[i].Offset(offset);
                    }

                    graphics.DrawPolygon(Pens.Lime, newPoints);
                }

                if (collidable?.ProjectileCollision != null)
                {
                    var newPoints = new PointF[collidable.ProjectileCollision.Points.Length];
                    var oldPoints = collidable.ProjectileCollision.Points;

                    for (int i = 0; i < newPoints.Length; i++)
                    {
                        newPoints[i] = oldPoints[i].Offset(offset);
                    }

                    graphics.DrawPolygon(Pens.Red, newPoints);
                }
            }
        }

        /// <summary>
        /// Draws the model for the truck.
        /// </summary>
        /// <param name="graphics">The surface to draw to.</param>
        /// <param name="model">The truck model.</param>
        public void DrawTruck(Graphics graphics, PointF[] model)
        {
            graphics.FillPolygon(Brushes.DodgerBlue, model);
        }

        /// <summary>
        /// Draws the specified contrail.
        /// </summary>
        /// <param name="graphics">The surface to draw to.</param>
        /// <param name="line">The contrail's line.</param>
        /// <param name="kind">The kind of contrail (specifies the effect).</param>
        public void DrawContrail(Graphics graphics, Line line, ContrailKind kind)
        {
            Pen pen = null;

            switch (kind)
            {
                case ContrailKind.Blood: pen = bloodContrailPen; break;
                case ContrailKind.Bullet: pen = bulletContrailPen; break;
            }

            graphics.DrawLine(ShowDebugMarkings ? Pens.Red : pen, line);
        }

        /// <summary>
        /// Draws a bipeds pistol.
        /// </summary>
        /// <param name="graphics">The surface to draw to.</param>
        /// <param name="spriteLine">The pistol line.</param>
        public void DrawPistol(Graphics graphics, Line spriteLine)
        {
            graphics.DrawLine(pistolPen, spriteLine);
        }

        /// <summary>
        /// Draws a blocker sprite.
        /// </summary>
        /// <param name="graphics">The surface to draw to.</param>
        /// <param name="path">The path of the blocker sprite.</param>
        public void DrawBlocker(Graphics graphics, PointF[] path)
        {
            graphics.DrawPolygon(Pens.Red, path);
        }

        /// <summary>
        /// Draws the prompt that occurs when a player nears a specific vehicle.
        /// </summary>
        /// <param name="graphics">The surface to draw to.</param>
        /// <param name="text">The text to display as a prompt.</param>
        /// <param name="point">The point at which to draw the text.</param>
        public void DrawEnterVehicleText(Graphics graphics, string text, PointF point)
        {
            graphics.DrawString(text, enterVehicleFont, Brushes.GreenYellow, point);
        }

        /// <summary>
        /// Draws a biped from the specified dimensions.
        /// </summary>
        /// <param name="graphics">The surface to draw to.</param>
        /// <param name="headBounds">The bounds of the biped's head.</param>
        /// <param name="lines">The lines of the biped (limbs and shoulders)</param>
        public void DrawBiped(Graphics graphics, RectangleF headBounds, Line[] lines)
        {
            graphics.DrawLine(bipedTorsoPen, lines[0]);
            graphics.DrawLine(bipedArmsPen, lines[1]);
            graphics.DrawLine(bipedArmsPen, lines[2]);
            graphics.FillEllipse(Brushes.White, headBounds);
        }

        public void Dispose()
        {
            bulletContrailPen.Dispose();
            bloodContrailPen.Dispose();
            pistolPen.Dispose();
            debugPen.Dispose();
            bipedTorsoPen.Dispose();
            bipedArmsPen.Dispose();
            enterVehicleFont.Dispose();
            debugFont.Dispose();
        }
    }
}
