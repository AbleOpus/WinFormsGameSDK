using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace WinFormsGameSDK.Sprites
{
    /// <summary>
    /// Represents a sprite capable of being collided with.
    /// </summary>
    public abstract class CollidableSprite : Sprite
    {
        /// <summary>
        /// Gets the move speed of the sprite.
        /// </summary>
        public float MoveSpeed { get; protected set; }

        /// <summary>
        /// Gets the move increment of the sprite.
        /// </summary>
        protected float MoveIncrement => MoveSpeed / GameSessionBase.TickRate;

        /// <summary>
        /// Gets or sets the mass of the sprite.
        /// </summary>
        public float Mass { get; set; }

        /// <summary>
        /// Gets the movement collision geometry of the sprite.
        /// </summary>
        public Geometry MovementCollision { get; protected set; }

        /// <summary>
        /// Gets the projectile collision geometry of the sprite.
        /// </summary>
        public Geometry ProjectileCollision { get; protected set; }

        /// <summary>
        /// Gets the model geometry of the sprite.
        /// </summary>
        public Geometry Model { get; protected set; }

        /// <summary>
        /// Updates this sprite (typically called every game loop iteration).
        /// </summary>
        public override void Update()
        {
            base.Update();
            MovementCollision?.MoveTransform(Position);
            ProjectileCollision?.MoveTransform(Position);
            Model?.MoveTransform(Position);

            if (MovementCollision != null)
            {
                MovementCollision.FacingDegree = Vector.FacingDegree;
            }

            if (ProjectileCollision != null)
            {
                ProjectileCollision.FacingDegree = Vector.FacingDegree;
            }

            if (Model != null)
            {
                Model.FacingDegree = Vector.FacingDegree;
            }
        }

        /// <summary>
        /// Move this sprite away from the specified point.
        /// </summary>
        /// <param name="point">The point to move away from.</param>
        private void MoveOppositeOfPoint(PointF point)
        {
            Vector2D vector = new Vector2D(point);
            vector.FaceTarget(MovementCollision.Bounds.GetCenter());
            float lastAngle = FacingDegree;
            FacingDegree = vector.FacingDegree;
            Vector.Project(1);
            FacingDegree = lastAngle;
            OnPositionChanged();
        }

        /// <summary>
        /// Rotates this sprite in a constrained fashion.
        /// </summary>
        /// <param name="facingDegree">The and to face the sprite (in degrees).</param>
        /// <param name="hitExclusion">The sprites to not calculate hit tests on.</param>
        public void RotateConstrained(float facingDegree, params CollidableSprite[] hitExclusion)
        {
            if (facingDegree != FacingDegree)
            {
                if (MovementCollision == null)
                {
                    FacingDegree = facingDegree;
                    return;
                }

                var cloned = MovementCollision.Clone();
                cloned.FacingDegree = facingDegree;

                foreach (var collidable in SpriteManager.MovementBlocking)
                {
                    if (hitExclusion != null && hitExclusion.Contains(collidable)) continue;
                    if (collidable == this) continue;
                    var collidePoint = cloned.CollidesWith(collidable.MovementCollision);

                    if (collidePoint != PointF.Empty)
                    {
                        if (Mass >= collidable.Mass && collidable.Mass != 0)
                        {
                            // PushSprite(collidable);
                        }

                        if (Mass < collidable.Mass || collidable.Mass == 0)
                        {
                            MoveOppositeOfPoint(collidePoint);
                        }
                    }
                }

                FacingDegree = facingDegree;
            }
        }

        /// <summary>
        /// Pushes the specified sprite away from this instance.
        /// </summary>
        /// <param name="sprite">The sprite to push.</param>
        private void PushSprite(CollidableSprite sprite)
        {
            float lastDegree = sprite.FacingDegree;
            sprite.Vector.FaceTarget(Vector);
            sprite.Vector.Project(-1);
            sprite.Vector.FacingDegree = lastDegree;
        }

        public CollidableSprite MoveConstrained(float newX, float newY, params CollidableSprite[] excludedHitTest)
        {
            if (MovementCollision == null)
            {
                Position = new PointF(newX, newY);
                return null;
            }

            bool shouldMoveX = true;
            bool shouldMoveY = true;
            CollidableSprite collidesWith = null;

            foreach (var sprite in SpriteManager.MovementBlocking)
            {
                if (sprite.MovementCollision == null || sprite == this) continue;
                if (excludedHitTest != null && excludedHitTest.Contains(sprite)) continue;
                
                PointF collidePoint = sprite.MovementCollision.CollidesWith(MovementCollision);

                if (collidePoint != PointF.Empty)
                {
                    collidesWith = sprite;

                    if (Mass < sprite.Mass || sprite.Mass == 0)
                    {
                        MoveOppositeOfPoint(collidePoint);
                        shouldMoveY = false;
                        shouldMoveX = false;
                    }
                    else if (Mass >= sprite.Mass && sprite.Mass != 0)
                    {
                        PushSprite(sprite);
                    }
                    else
                    {
                        shouldMoveY = false;
                        shouldMoveX = false;
                    }
                }
            }

            if (shouldMoveY) Y = newY;
            if (shouldMoveX) X = newX;
            return collidesWith;
        }

        private void MoveGlobal(MoveDirection direction)
        {
            if (direction.HasFlag(MoveDirection.Forwards))
            {
                float newY = Y - MoveIncrement;
                MoveConstrained(X, newY);
            }
            if (direction.HasFlag(MoveDirection.Backwards))
            {
               float newY = Y + MoveIncrement;
                  MoveConstrained(X, newY);
            }
            if (direction.HasFlag(MoveDirection.Left))
            {
                float newX = X - MoveIncrement;
                MoveConstrained(newX, Y);
            }
            if (direction.HasFlag(MoveDirection.Right))
            {
                float newX = X + MoveIncrement;
                MoveConstrained(newX, Y);
            }

            //switch (direction)
            //{
            //    case MoveDirection.Forwards:
            //        float newY = Y - MoveIncrement;
            //        MoveConstrained(X, newY);
            //        break;
            //    case MoveDirection.Backwards:
            //        newY = Y + MoveIncrement;
            //        MoveConstrained(X, newY);
            //        break;
            //    case MoveDirection.Left:
            //        float newX = X - MoveIncrement;
            //        MoveConstrained(newX, Y);
            //        break;
            //    case MoveDirection.Right:
            //        newX = X + MoveIncrement;
            //        MoveConstrained(newX, Y);
            //        break;
            //}
        }

        protected CollidableSprite MoveLocal(MoveDirection direction, params CollidableSprite[] excludedHitTests)
        {
            Vector2D vector = Vector.Clone();

            if (direction.HasFlag(MoveDirection.Forwards))
            {
                vector.Project(MoveIncrement);
                return MoveConstrained(vector.X, vector.Y, excludedHitTests);
            }
            if (direction.HasFlag(MoveDirection.Backwards))
            {
                vector.Project(-MoveIncrement);
                return MoveConstrained(vector.X, vector.Y, excludedHitTests);
            }

            return null;
            //if (direction.HasFlag(MoveDirection.Left))
            //{
            //    vector.FacingDegree += 90;
            //    vector.Project(MoveIncrement);
            //     Vector.Position = vector.Position;
            //    MoveConstrained(vector.X, vector.Y, excludedHitTests);
            //}
            //if (direction.HasFlag(MoveDirection.Right))
            //{
            //    vector.FacingDegree -= 90;
            //    vector.Project(MoveIncrement);
            //    //Vector.Position = vector.Position;
            //    MoveConstrained(vector.X, vector.Y, excludedHitTests);
            //}

            //switch (direction)
            //{
            //    case MoveDirection.Forwards:
            //        vector.Project(MoveIncrement);
            //        MoveConstrained(vector.X, vector.Y);
            //        break;

            //    case MoveDirection.Backwards:
            //        vector.Project(-MoveIncrement);
            //        MoveConstrained(vector.X, vector.Y);
            //        break;

            //    case MoveDirection.Left:
            //        vector.FacingDegree += 90;
            //        vector.Project(MoveIncrement);
            //        // Vector.Position = vector.Position;
            //        MoveConstrained(vector.X, vector.Y);
            //        break;

            //    case MoveDirection.Right:
            //        vector.FacingDegree -= 90;
            //        vector.Project(MoveIncrement);
            //        //Vector.Position = vector.Position;
            //        MoveConstrained(vector.X, vector.Y);
            //        break;
            //}
        }

        public void Move(MoveDirection direction, MovementKind kind)
        {
            if (direction == MoveDirection.None) return;

            if (kind == MovementKind.Global)
            {
                MoveGlobal(direction);
            }
            else
            {
                MoveLocal(direction);
            }
        }
    }
}
