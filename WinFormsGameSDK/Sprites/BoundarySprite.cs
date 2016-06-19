using System.Drawing;
using System.Drawing.Drawing2D;

namespace WinFormsGameSDK.Sprites
{
    /// <summary>
    /// Represents a sprite that acts as a movement boundary.
    /// </summary>
    public class BoundarySprite : CollidableSprite
    {
        public RectangleF InnerBounds { get; }

        public RectangleF OuterBounds { get; }

        public BoundarySprite(RectangleF boundary, float thickness)
        {
            InnerBounds = boundary;
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(boundary);
            boundary.Inflate(thickness, thickness);
            OuterBounds = boundary;
            path.AddRectangle(boundary);
            MovementCollision = new Geometry(path);
            ProjectileCollision = MovementCollision;
        }
    }
}
