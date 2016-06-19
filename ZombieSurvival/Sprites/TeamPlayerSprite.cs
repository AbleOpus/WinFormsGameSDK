using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using WinFormsGameSDK;
using WinFormsGameSDK.Sprites;

namespace ZombieSurvival.Sprites
{
    /// <summary>
    /// Represents a team NPC.
    /// </summary>
    class TeamPlayerSprite : PlayerSprite
    {
        private float aimPrecision = 5, turnSpeed = 200;
        private readonly Random random = new Random();
       // private ZombieSprite target;
        private readonly PlayerSprite toCover;
        private readonly IEnumerable<ZombieSprite> possibleTargets;
        private readonly Stopwatch getBehindWatch = new Stopwatch();
        private PointF moveToPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamPlayerSprite"/> class
        /// with the specified arguments.
        /// </summary>
        /// <param name="toCover">The player to provide cover for.</param>
        /// <param name="possibleTargets">The possible targets to shoot at.</param>
        public TeamPlayerSprite(PlayerSprite toCover,
            IEnumerable<ZombieSprite> possibleTargets) : base(0, 0, false)
        {
            Mass = 100;
            this.toCover = toCover;
            this.possibleTargets = possibleTargets;
            Position = GetCoverPosition();
            moveToPoint = GetCoverPosition();
            getBehindWatch.Start();
        }

        /// <summary>
        /// Gets the position in which the player moves to, to get cover.
        /// Currently it is behind another player.
        /// </summary>
        /// <returns>Gets the point to move to.</returns>
        private PointF GetCoverPosition()
        {
            Vector2D behindVector = toCover.Vector.Clone();
            behindVector.Project(-100);
            return behindVector.Position;
        }

        /// <summary>
        /// Gets the zombie that is closest to this player.
        /// </summary>
        /// <param name="distance">The distance the closest zombie is from this player.
        /// Returns int.MaxValue if no zombies are loaded.</param>
        /// <returns>The closest zombie sprite if a zombie is around, otherwise null.</returns>
        private ZombieSprite GetClosestZombie(out float distance)
        {
            float closestDistance = int.MaxValue;
            ZombieSprite closestZombie = null;

            foreach (var zombie in possibleTargets)
            {
                float distanceToPlayer = Vector.DistanceTo(zombie.Vector);

                if (zombie.Health > 0 && distanceToPlayer < closestDistance)
                {
                    closestDistance = distanceToPlayer;
                    closestZombie = zombie;
                }
            }

            distance = closestDistance;
            return closestZombie;
        }

        /// <summary>
        /// Shoots at the specified zombie.
        /// </summary>
        /// <param name="zombie">The zombie to shoot at.</param>
        private void ShootZombie(ZombieSprite zombie)
        {
            var desiredAngle = Position.AngleTo(zombie.Position);
            // RotateConstrained(desiredAngle);
            float angleDiff = FacingDegree - desiredAngle;

            if (Math.Abs(angleDiff) > 2)
            {
                float turnIncrement = turnSpeed / GameSessionBase.TickRate;

                if (angleDiff < 0)
                    RotateConstrained(FacingDegree + turnIncrement);
                else
                    RotateConstrained(FacingDegree - turnIncrement);

                Shooting = false;
            }
            else
            {
                Shooting = true;
                float deviation = random.Next(-(int)(aimPrecision / 2), (int)(aimPrecision / 2));
                Vector.FacingDegree += deviation;
            }
        }

        /// <summary>
        /// Updates this sprite (typically called every game loop iteration).
        /// </summary>
        public override void Update()
        {
            base.Update();
            float closestDistance;
            var closestZombie = GetClosestZombie(out closestDistance);
            bool retractingFromZombie = false;

            if (closestZombie != null)
            {
                if (closestDistance < 600)
                {
                    ShootZombie(closestZombie);
                }
                else
                {
                    Vector.FaceTarget(toCover.Vector);
                    Shooting = false;
                }

                if (closestDistance < 100)
                {
                    Vector2D vector = Vector.Clone();
                    vector.FaceTarget(closestZombie.Vector);
                    vector.Project(-MoveIncrement);
                    MoveConstrained(vector.X, vector.Y);
                    retractingFromZombie = true;
                }
            }
            else
            {
                Vector.FaceTarget(toCover.Vector);
                Shooting = false;
            }

            if (Vector.DistanceTo(moveToPoint) > 10 && !retractingFromZombie)
            {
                Vector2D moveVector = Vector.Clone();
                moveVector.FaceTarget(moveToPoint);
                moveVector.Project(MoveIncrement);
                MoveConstrained(moveVector.X, moveVector.Y);
            }

            if (getBehindWatch.ElapsedMilliseconds > 1000)
            {
                moveToPoint = GetCoverPosition();
                getBehindWatch.Restart();
            }
        }
    }
}
