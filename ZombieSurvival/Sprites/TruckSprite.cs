using System.Drawing;
using WinFormsGameSDK;

namespace ZombieSurvival.Sprites
{
    /// <summary>
    /// Represents a truck vehicle.
    /// </summary>
    class TruckSprite : VehicleSprite
    {
        private float truckWidth = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckSprite"/> class.
        /// </summary>
        public TruckSprite()
        {
            MoveSpeed = 0;
            MaxTurnRadius = 100;
            Mass = 1000;
            StraigtenTurnForce = 5;
            Acceleration = 400;
            TopSpeed = 20000;
            TurnSpeed = 100;
            Deceleration = 100;
            // Use the same geometry for all 2d models in this sprite.
            MovementCollision = Geometry.FromRectangle(0, 0, 200, truckWidth);
            ProjectileCollision = MovementCollision;
            Model = MovementCollision;
        }

        /// <summary>
        /// Gets the center position of where the driver sits.
        /// </summary>
        public override PointF GetDriverPosition()
        {
            Vector2D temp = Vector.Clone();
            temp.Project(truckWidth / 2);
            return temp.Position;
        }

        /// <summary>
        /// Gets the center position of where the driver will end up after exiting
        /// the driver's seat.
        /// </summary>
        public override PointF GetDriverExitPoint()
        {
            Vector2D temp = Vector.Clone();
            temp.FacingDegree -= 90;
            temp.Project(truckWidth);
            return temp.Position;
        }
    }
}
