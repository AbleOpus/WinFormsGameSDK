using System.Collections.Generic;
using System.Drawing;
using WinFormsGameSDK;

namespace ZombieSurvival.Sprites
{
    /// <summary>
    /// Represents a flesh-eating zombie.
    /// </summary>
    public class ZombieSprite : BipedSprite
    {
        /// <summary>
        /// Gets or sets the players in which this zombie can target.
        /// </summary>
        public IEnumerable<PlayerSprite> Targets { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZombieSprite"/> with the 
        /// specified arguments.
        /// </summary>
        /// <param name="targets">The players in which this zombie can target.</param>
        /// <param name="position">The start position of the zombie.</param>
        public ZombieSprite(IEnumerable<PlayerSprite> targets, PointF position)
        {
            Position = position;
            Targets = targets;
            MoveSpeed = 50;
            Mass = 100;
        }

        /// <summary>
        /// Makes the zombie face the specified point.
        /// </summary>
        /// <param name="point">The point to face.</param>
        public void FaceTarget(PointF point)
        {
            Vector2D temp = Vector.Clone();
            temp.FaceTarget(point);

            if (temp.FacingDegree != Vector.FacingDegree)
            {
                FacingDegree = temp.FacingDegree;
            }
        }

        /// <summary>
        /// Updates this sprite (typically called every game loop iteration).
        /// </summary>
        public override void Update()
        {
            base.Update();
            RightHand = RightShoulder.Clone();
            RightHand.FacingDegree = Vector.FacingDegree;
            RightHand.Project(ArmLength);

            LeftHand = LeftShoulder.Clone();
            LeftHand.FacingDegree = Vector.FacingDegree;
            LeftHand.Project(ArmLength);

            float distance;
            var nearestPlayer = GetNearestSprite(Targets, out distance);

            if (nearestPlayer != null && distance < 600)
            {
                FaceTarget(nearestPlayer.Position);
                var temp = Vector.Clone();
                temp.Project(MoveSpeed / GameSessionBase.TickRate);
                Move(MoveDirection.Forwards, MovementKind.Local);
            }
        }
    }
}
