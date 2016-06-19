using System.Drawing;
using WinFormsGameSDK;
using WinFormsGameSDK.Drawing;
using WinFormsGameSDK.Sprites;

namespace ZombieSurvival.Sprites
{
    /// <summary>
    /// Represents a bipedal, collidable sprite.
    /// </summary>
    public abstract class BipedSprite : CollidableSprite
    {
        /// <summary>
        /// Gets the fatness of the biped's shoulders.
        /// </summary>
        public static float ShoulderFatness { get; } = 8;

        /// <summary>
        /// Gets the width of the biped's shoulders.
        /// </summary>
        public static float ShoulderWidth { get; } = 30;

        /// <summary>
        /// Gets the diameter of the biped's head.
        /// </summary>
        public static float HeadDiameter { get; } = 25;

        private int health = 100;
        /// <summary>
        /// Gets or sets the health of the biped as a percentage from 0-100.
        /// </summary>
        public int Health
        {
            get { return health; }
            set
            {
                health = value;

                if (health <= 0)
                    Expired = true;
            }
        }

        /// <summary>
        /// Gets the length of the biped's arms.
        /// </summary>
        public float ArmLength { get; protected set; } = 27;

        /// <summary>
        /// Gets whether the biped is dead.
        /// </summary>
        public bool IsDead => Health <= 0;

        /// <summary>
        /// Gets the boundaries of the biped's head.
        /// </summary>
        public RectangleF HeadBounds => 
            new RectangleF(X - HeadDiameter / 2, Y - HeadDiameter / 2, HeadDiameter, HeadDiameter);

        /// <summary>
        /// Gets the vector of the biped's left hand.
        /// </summary>
        public Vector2D LeftHand { get; protected set; }

        /// <summary>
        /// Gets the vector of the biped's right hand.
        /// </summary>
        public Vector2D RightHand { get; protected set; }

        /// <summary>
        /// Gets the vector of the biped's right shoulder.
        /// </summary>
        public Vector2D RightShoulder { get; protected set; }

        /// <summary>
        /// Gets the vector of the biped's left shoulder.
        /// </summary>
        public Vector2D LeftShoulder { get; protected set; }

        public BipedSprite()
        {
            //float collDim = ShoulderWidth + ShoulderFatness * 2;
            MovementCollision = Geometry.CreateCircle(8, ShoulderWidth);
            ProjectileCollision = Geometry.FromRectangle(0, 0, 15, 35);
            ProjectileCollision.TransformPoint = new PointF(ProjectileCollision.TransformPoint.X + 5, ProjectileCollision.TransformPoint.Y);
        }

        protected virtual void PlaceTorso()
        {
            RightShoulder = Vector.Clone();
            RightShoulder.FacingDegree -= 90;
            RightShoulder.Project(ShoulderWidth / 2);

            LeftShoulder = Vector.Clone();
            LeftShoulder.FacingDegree += 90;
            LeftShoulder.Project(ShoulderWidth / 2);
        }

        /// <summary>
        /// Gets the geometric figure of the biped as an array of lines.
        /// </summary>
        public Line[] GetFigure()
        {
            return new[]
            {
                new Line(RightShoulder.Position, LeftShoulder.Position),
                new Line(LeftShoulder.Position, LeftHand.Position),
                new Line(RightShoulder.Position, RightHand.Position)
            };
        }

        /// <summary>
        /// Updates this sprite (typically called every game loop iteration).
        /// </summary>
        public override void Update()
        {
            base.Update();
            PlaceTorso();
        }
    }
}
