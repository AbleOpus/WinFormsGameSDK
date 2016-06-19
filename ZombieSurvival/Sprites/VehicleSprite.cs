using System.Diagnostics;
using System.Drawing;
using System.Text;
using WinFormsGameSDK;
using WinFormsGameSDK.Forms;
using WinFormsGameSDK.Sprites;

namespace ZombieSurvival.Sprites
{
    /// <summary>
    /// Represents a drivable vehicle.
    /// </summary>
    public abstract class VehicleSprite : CollidableSprite
    {
        //public PointF DriverPosition { get; protected set; }

        /// <summary>
        /// Gets or sets the maximum radius at which to turn.
        /// </summary>
        public float MaxTurnRadius { get; set; }

        /// <summary>
        /// Gets or sets the intensity of the vehicles tendency to straighten its turn radius.
        /// </summary>
        public float StraigtenTurnForce { get; set; }

        /// <summary>
        /// Gets how much to increment the turn.
        /// </summary>
        public float TurnIncrement { get; protected set; }

        /// <summary>
        /// Gets or sets the direction in which the vehicle is moving.
        /// </summary>
        public MoveDirection MoveDirection { get; set; }

        /// <summary>
        /// Gets or sets the distance a player has to be within, for the player
        /// to enter the driver's seat.
        /// </summary>
        public float DistanceToEnterSeat { get; set; } = 30;

        /// <summary>
        /// Gets or sets the player that is driving the vehicle (the driver's seat).
        /// </summary>
        public PlayerSprite Driver { get; set; }

        /// <summary>
        /// Gets the turn speed of the vehicle.
        /// </summary>
        public float TurnSpeed { get; protected set; }

        /// <summary>
        /// Gets how fast the vehicle picks up speed.
        /// </summary>
        public float Acceleration { get; protected set; }

        /// <summary>
        /// Gets how fast the vehicle looses speed when it is not throttling.
        /// </summary>
        public float Deceleration { get; protected set; }

        /// <summary>
        /// Gets the maximum speed in which the vehicle can travel.
        /// </summary>
        public float TopSpeed { get; protected set; }

        /// <summary>
        /// Gets the center position of the driver's seat.
        /// </summary>
        public abstract PointF GetDriverPosition();

        /// <summary>
        /// Gets the center position of where the driver will end up after exiting
        /// the driver's seat.
        /// </summary>
        public abstract PointF GetDriverExitPoint();

        /// <summary>
        /// Updates this sprite (typically called every game loop iteration).
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (Driver != null)
            {
                Driver.Position = GetDriverPosition();
                Driver.FacingDegree = FacingDegree;
            }

            float topSpeed = TopSpeed / GameSessionBase.TickRate;
            float accel = Acceleration / GameSessionBase.TickRate;

            if (MoveDirection.HasFlag(MoveDirection.Left))
            {
                TurnIncrement = (TurnSpeed / GameSessionBase.TickRate);

            }
            if (MoveDirection.HasFlag(MoveDirection.Right))
            {
                TurnIncrement = -(TurnSpeed / GameSessionBase.TickRate);
            }

            if (MoveDirection.HasFlag(MoveDirection.Forwards))
            {
                if (MoveSpeed > topSpeed)
                {
                    MoveSpeed = topSpeed;
                }
                else
                {
                    MoveSpeed += accel;
                }
            }
            else if (MoveDirection.HasFlag(MoveDirection.Backwards))
            {
                if (MoveSpeed < -topSpeed)
                {
                    MoveSpeed = -topSpeed;
                }
                else
                {
                    MoveSpeed -= accel;
                }
            }

            // Auto restore turn to 0.
            if (TurnIncrement > 0)
            {
                TurnIncrement -= StraigtenTurnForce / GameSessionBase.TickRate;
                if (TurnIncrement > MaxTurnRadius)
                    TurnIncrement = MaxTurnRadius;
            }
            else if (TurnIncrement < 0)
            {
                TurnIncrement += StraigtenTurnForce / GameSessionBase.TickRate;
                if (TurnIncrement < -MaxTurnRadius)
                    TurnIncrement = -MaxTurnRadius;
            }

            // Auto restore speed to 0.
            if (MoveSpeed > 0)
            {
                MoveSpeed -= Deceleration / GameSessionBase.TickRate;
            }

            if (MoveSpeed < 0)
            {
                MoveSpeed += Deceleration / GameSessionBase.TickRate;
            }

            RotateConstrained(FacingDegree + TurnIncrement, Driver);
            var collidesWith = MoveLocal(MoveDirection.Forwards, Driver);

            if (collidesWith != null)
            {
                MoveSpeed = 0;
            }
        }
    }
}
