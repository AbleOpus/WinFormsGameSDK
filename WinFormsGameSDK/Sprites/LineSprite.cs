using WinFormsGameSDK.Drawing;

namespace WinFormsGameSDK.Sprites
{
    /// <summary>
    /// A sprite that can be effectively represented by two points. Such as a
    /// bullet contrail, or simple blood contrail.
    /// </summary>
    public abstract class LineSprite : Sprite
    {
        /// <summary>
        /// Gets or sets the vector at the end of the line.
        /// </summary>
        public Vector2D EndVector { get; set; } = new Vector2D();

        /// <summary>
        /// Gets or sets the line that gives the sprite form.
        /// </summary>
        public Line Line => new Line(Vector.Position, EndVector.Position);

        protected LineSprite() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSprite"/> 
        /// with the specified arguments.
        /// </summary>
        /// <param name="startVector">The star vector of the line.</param>
        /// <param name="endVector">The end vector of the line.</param>
        protected LineSprite(Vector2D startVector, Vector2D endVector) : base(startVector)
        {
            EndVector = endVector;
        }
    }
}