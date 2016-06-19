using System.Drawing;
using WinFormsGameSDK;
using WinFormsGameSDK.Sprites;

namespace ZombieSurvival.Sprites
{
    /// <summary>
    /// Represents sprites that block other sprites.
    /// </summary>
    public class BlockerSprite : CollidableSprite
    {
        public BlockerSprite(RectangleF bounds)
        {
            MovementCollision = Geometry.FromRectangle(bounds);
            ProjectileCollision = MovementCollision;
        }

        public BlockerSprite(float x, float y, float width, float height)
            : this(new RectangleF(x, y, width, height))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockerSprite"/> class
        /// with the specified argument.
        /// </summary>
        /// <param name="path">The geometry of the sprite.</param>
        public BlockerSprite(PointF[] path)
        {
            MovementCollision = new Geometry(path);
            ProjectileCollision = MovementCollision;
        }

        /// <summary>
        /// Creates a rectangular enclosure from the specified bounds.
        /// </summary>
        /// <param name="bounds">The size and location of the enclosure.</param>
        /// <param name="thickness">The thickness of the boundary. The barrier width.</param>
        /// <returns>The blocker sprites that are put together to form a barrier.</returns>
        public static BlockerSprite[] CreateRectangularEnclosure(RectangleF bounds, float thickness)
        {
            PointF topLeft = new PointF(bounds.X - thickness, bounds.Y - thickness);

            return new[]
            {
                // Left wall.
               new BlockerSprite(topLeft.X, topLeft.Y, thickness, bounds.Height + thickness * 2),
               // Top wall.
               new BlockerSprite(topLeft.X, topLeft.Y, bounds.Width + thickness * 2, thickness),
               // Bottom wall.
               new BlockerSprite(topLeft.X, bounds.Y + bounds.Height, bounds.Width + thickness * 2, thickness),
               // Right wall.
               new BlockerSprite(bounds.Right, bounds.Y - thickness, thickness, bounds.Height + thickness * 2)
        };
        }
    }
}
