using System;
using System.Collections.Generic;
using System.Drawing;

namespace WinFormsGameSDK.Sprites
{
    /// <summary>
    /// The base implementation of a game sprite.
    /// </summary>
    [Serializable]
    public abstract class Sprite
    {
        /// <summary>
        /// Gets or sets the sprite identifier. Mainly for debugging purposes.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the facing direction, in degrees, of the sprite.
        /// </summary>
        public float FacingDegree
        {
            get { return Vector.FacingDegree; }
            set
            {
                if (value != Vector.FacingDegree)
                {
                    Vector.FacingDegree = value;
                    OnFacingDegreeChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the animation speed of this sprite.
        /// </summary>
        public float BaseAnimationSpeed { get; set; }

        /// <summary>
        /// Gets the direction and position of this sprite.
        /// </summary>
        public Vector2D Vector { get; } = new Vector2D();

        /// <summary>
        /// Gets whether this sprite has expired (is no longer useful).
        /// </summary>
        public virtual bool Expired { get; protected set; }

        /// <summary>
        /// Gets or sets the position of this sprite.
        /// </summary>
        public PointF Position
        {
            get { return Vector.Position; }
            set
            {
                if (Vector.Position != value)
                {
                    Vector.Position = value;
                   // OnPositionChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the x position of the sprite.
        /// </summary>
        public float X
        {
            get { return Position.X; }
            set
            {
                if (value != Vector.X)
                {
                    Vector.X = value;
                    OnPositionChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the y position of the sprite.
        /// </summary>
        public float Y
        {
            get { return Vector.Y; }
            set
            {
                if (value != Vector.Y)
                {
                    Vector.Y = value;
                    OnPositionChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class
        /// with the specified argument.
        /// </summary>
        /// <param name="vector">The starting vector of the sprite.</param>
        protected Sprite(Vector2D vector)
        {
            Vector = vector;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class
        /// with the specified argument.
        /// </summary>
        /// <param name="position">The starting position of the sprite.</param>
        protected Sprite(PointF position)
        {
            Position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class
        /// with the specified argument.
        /// </summary>
        /// <param name="x">The starting x-position of the sprite.</param>
        /// <param name="y">The starting y-position of the sprite.</param>
        protected Sprite(float x, float y) : this(new PointF(x, y)) { }

        protected Sprite()
        {
        }

        /// <summary>
        /// Gets the sprite that is nearest this sprite.
        /// </summary>
        /// <param name="sprites">The sprites to evaluate the distance of.</param>
        /// <param name="distance">The distance of the nearest sprite. The value will
        /// be -1 if no sprite are provided.</param>
        /// <returns>Null, if no sprites provided, otherwise, the nearest sprite.</returns>
        public Sprite GetNearestSprite(IEnumerable<Sprite> sprites, out float distance)
        {
            Sprite nearestSprite = null;
            distance = -1;

            foreach (var sprite in sprites)
            {
                var tempDist = Vector.DistanceTo(sprite.Vector);

                if (nearestSprite == null || tempDist < distance)
                {
                    nearestSprite = sprite;
                    distance = tempDist;
                }
            }

            return nearestSprite;
        }

        /// <summary>
        /// Update this sprite (typically called on every game loop iteration).
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Raises when the facing degree of the sprite has changed.
        /// </summary>
        protected virtual void OnFacingDegreeChanged() { }

        /// <summary>
        /// Raises when the position of the sprite has changed.
        /// </summary>
        public virtual void OnPositionChanged() { }
    }
}
