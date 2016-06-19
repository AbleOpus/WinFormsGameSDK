using System;
using System.Drawing;

namespace WinFormsGameSDK
{
    /// <summary>
    /// Represents a position and direction.
    /// </summary>
    [Serializable]
    public class Vector2D : ICloneable<Vector2D>
    {
        /// <summary>
        /// Gets or sets the rotation of this vector in radians.
        /// </summary>
        public float FacingRadian { get; set; }

        /// <summary>
        /// Gets or sets the rotation of this vector in degrees.
        /// </summary>
        public float FacingDegree
        {
            get { return GameMath.RadianToDegree(FacingRadian); }
            set { FacingRadian = GameMath.DegreeToRadian(value); }
        }

        /// <summary>
        /// Gets or sets the x and y position of this vector.
        /// </summary>
        public PointF Position { get; set; }

        /// <summary>
        /// Gets or sets the X position of the vector.
        /// </summary>
        public float X
        {
            get { return Position.X; }
            set { Position = new PointF(value, Y); }
        }

        /// <summary>
        /// Gets or sets the Y position of the vector.
        /// </summary>
        public float Y
        {
            get { return Position.Y; }
            set { Position = new PointF(X, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> class
        /// with the specified arguments.
        /// </summary>
        /// <param name="x">The x position of the vector.</param>
        /// <param name="y">The y position of the vector.</param>
        /// <param name="facingDegrees">The facing direction of the vector in degrees.</param>
        public Vector2D(float x, float y, float facingDegrees = 0) 
            : this(new PointF(x, y), facingDegrees) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> class
        /// with the specified arguments.
        /// </summary>
        /// <param name="position">The position of the vector.</param>
        /// <param name="facingDegrees">The facing direction of the vector in degrees.</param>
        public Vector2D(PointF position, float facingDegrees = 0)
        {
            Position = position;
            FacingDegree = facingDegrees;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2D"/> class.
        /// </summary>
        public Vector2D() {  }

        /// <summary>
        /// Creates a new vector based on this instance, and projects it forward in 
        /// the direction it is facing.
        /// </summary>
        /// <param name="distance">The distance to project the vector.</param>
        public void Project(float distance)
        {
            //var indicator = new Vector2D(0, distance).Rotate(FacingRadian);
            //Position = new PointF(X + indicator.X, Y + indicator.Y);

            Vector2D move = new Vector2D(0, distance).RotateDegrees(FacingDegree);

            // add to position
            AddEquals(move);
        }

        /// <summary>
        /// Adds the specified vector to this instance.
        /// </summary>
        /// <returns>The results of the vectors being added together.</returns>
        public Vector2D AddEquals(Vector2D other)
        {
            X += other.X;
            Y += other.Y;
            return this;
        }

        /// <summary>
        /// Turns this vector to face a target vector.
        /// </summary>
        /// <param name="target">The vector to face towards.</param>
        public void FaceTarget(Vector2D target)
        {
            FaceTarget(target.Position);
        }

        /// <summary>
        /// Turns this vector to face a target point.
        /// </summary>
        /// <param name="target">The point to face towards.</param>
        public void FaceTarget(PointF target)
        {
            FacingDegree = Position.AngleTo(target);
        }

        /// <summary>
        /// Creates a new vector based on the position of this instance and
        /// the specified vector.
        /// </summary>
        /// <returns>The results of the vectors being added together.</returns>
        public Vector2D Add(Vector2D other)
        {
            return new Vector2D(X + other.X, Y + other.Y);
        }

        /// <summary>
        /// Multiplies this vector by the specified value.
        /// </summary>
        public Vector2D Multiply(float scalar)
        {
            return new Vector2D(X * scalar, Y * scalar);
        }

        /// <summary>
        /// Creates a new vector based on this instance, with the specified rotation applied.
        /// </summary>
        /// <param name="angle">The angle to rotate the vector at (in radians).</param>
        public Vector2D Rotate(float angle)
        {
            float c = (float)Math.Cos(angle);
            float s = (float)Math.Sin(angle);
            float rx = X * s - Y * c;
            float ry = Y * s + X * c;
            return new Vector2D(rx, ry);
        }

        /// <summary>
        /// Flips this facing degree by 180.
        /// </summary>
        public void Flip()
        {
            FacingDegree += 180;
        }

        /// <summary>
        /// Creates a new vector based on this instance, with the specified rotation applied.
        /// </summary>
        /// <param name="degrees">The angle to rotate the vector at (in degrees).</param>
        public Vector2D RotateDegrees(float degrees)
        {
            return Rotate(GameMath.DegreeToRadian(degrees));
        }

        /// <summary>
        /// The distance between this instance and the specified point.
        /// </summary>
        public float DistanceTo(PointF other)
        {
            double dx = other.X - X, dy = other.Y - Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// The distance between this instance and the specified vector.
        /// </summary>
        public float DistanceTo(Vector2D vector)
        {
            return DistanceTo(vector.Position);
        }

        /// <summary>
        /// Creates a clone of this <see cref="Vector2D"/> instance.
        /// </summary>
        /// <returns>The cloned instance.</returns>
        public Vector2D Clone()
        {
            return new Vector2D(Position, FacingDegree);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"{X}, {Y}";
        }
    }
}
