using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using WinFormsGameSDK;

namespace ZombieSurvival.Sprites
{
    /// <summary>
    /// Represents a sprite that is controlled by a user or AI.
    /// </summary>
    public class PlayerSprite : BipedSprite
    {
        private readonly Stopwatch fireRateWatch = new Stopwatch();
        //private double lastFireTime;
        private ShootingAnimationState shootingAnimState;
        private readonly float maxArmLength;
        private readonly float minArmLength;

        /// <summary>
        /// Gets whether the player is currently driving.
        /// </summary>
        public bool IsDriving => ParentVehicle != null;

        /// <summary>
        /// Gets or sets the vehicle the player is within.
        /// </summary>
        public VehicleSprite ParentVehicle { get; set; }

        /// <summary>
        /// Gets or sets the vehicle that is near the player.
        /// </summary>
        public VehicleSprite NearbyVehicle { get; set; }

        private MoveDirection moveDirection;
        /// <summary>
        /// Gets or sets the move direction of player.
        /// </summary>
        public MoveDirection MoveDirection
        {
            get { return moveDirection; }
            set
            {
                moveDirection = value;

                if (ParentVehicle != null)
                    ParentVehicle.MoveDirection = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the player is shooting.
        /// </summary>
        public bool Shooting { get; set; }

        /// <summary>
        /// Gets the bullet vectors of the bullets that this sprite has fired.
        /// </summary>
        public Stack<Vector2D> BulletVectors { get; } = new Stack<Vector2D>();

        /// <summary>
        /// Gets the gun sprite that the player is currently wielding.
        /// </summary>
        public GunSprite Gun { get; set; } = new PistolSprite();

        /// <summary>
        /// Gets whether this player is local or remote.
        /// </summary>
        public bool IsLocal { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerSprite"/> class
        /// with the specified arguments.
        /// </summary>
        /// <param name="x">The starting x position of the player.</param>
        /// <param name="y">The starting y position of the player.</param>
        /// <param name="isLocal">Whether the player is local or remote.</param>
        public PlayerSprite(float x, float y, bool isLocal)
        {
            IsLocal = isLocal;
            minArmLength = ArmLength - 6;
            maxArmLength = ArmLength;
            Position = new PointF(x, y);
            MoveSpeed = 150;
            Mass = 200;
            fireRateWatch.Start();
        }

        /// <summary>
        /// Rotates the player by the specified angle.
        /// </summary>
        /// <param name="angle">The angle to rotate the player by.</param>
        public void Rotate(float angle)
        {
            if (!IsDriving)
            {
                RotateConstrained(angle);
            }
        }

        /// <summary>
        /// Updates this sprite (typically called every game loop iteration).
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (!IsDriving)
                Move(MoveDirection, MovementKind.Global);
            //var array = Enum.GetValues(typeof(MoveDirection));
            //foreach (var item in array)
            //{

            //    var flag = (MoveDirection) item;
            //    if (MoveDirection.HasFlag(flag))
            //        Move(flag, MovementType.Global);
            //}

            if (Shooting && fireRateWatch.ElapsedMilliseconds >= Gun.ShootInterval)
            {
                fireRateWatch.Restart();
                BulletVectors.Push(RightHand.Clone());
                shootingAnimState = ShootingAnimationState.Retracting;
            }

            RightHand = Vector.Clone();
            RightHand.Project(ArmLength);
            LeftHand = RightHand.Clone();
            float shootAnimSpeed = 130f / GameSessionBase.TickRate;

            switch (shootingAnimState)
            {
                case ShootingAnimationState.Retracting:
                    ArmLength -= shootAnimSpeed;

                    if (ArmLength < minArmLength)
                    {
                        shootingAnimState = ShootingAnimationState.Restoring;
                    }
                    break;

                case ShootingAnimationState.Restoring:
                    ArmLength += shootAnimSpeed;

                    if (ArmLength > maxArmLength)
                    {
                        ArmLength = maxArmLength;
                        shootingAnimState = ShootingAnimationState.None;
                    }
                    break;
            }

            Gun.Vector.FacingDegree = RightHand.FacingDegree;
            Gun.Position = RightHand.Position;
            Gun.Update();
        }
    }
}
