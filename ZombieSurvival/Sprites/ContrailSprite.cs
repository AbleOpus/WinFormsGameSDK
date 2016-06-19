using System.Diagnostics;
using WinFormsGameSDK;
using WinFormsGameSDK.Sprites;

namespace ZombieSurvival.Sprites
{
    /// <summary>
    /// Represents a linear contrail.
    /// </summary>
    class ContrailSprite : LineSprite
    {
        private readonly Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Gets the kind of contrail.
        /// </summary>
        public ContrailKind Kind { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContrailSprite"/> class with the specified arguments.
        /// </summary>
        /// <param name="startVector">The start vector of the contrail's line.</param>
        /// <param name="endVector">The end vector of the contrail's line.</param>
        /// <param name="kind">The kinds of contrail.</param>
        public ContrailSprite(Vector2D startVector, Vector2D endVector, ContrailKind kind) 
            : base(startVector, endVector)
        {
            Kind = kind;
            BaseAnimationSpeed = kind == ContrailKind.Bullet ? 3000 : 1000;
            stopwatch.Start();
        }

        /// <summary>
        /// Update this sprite (typically called on every game loop iteration).
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (Expired) return;

            if (Kind == ContrailKind.Bullet)
            {
                Vector.Project(BaseAnimationSpeed/ GameSessionBase.TickRate);

                if (Vector.DistanceTo(EndVector) < 50)
                {
                    Expired = true;
                }
            }
            else if (Kind == ContrailKind.Blood)
            {
                EndVector.Project(BaseAnimationSpeed / GameSessionBase.TickRate);

                if (Vector.DistanceTo(EndVector) > 100)
                {
                    Expired = true;
                }
            }

            if (stopwatch.ElapsedMilliseconds > 200)
            {
                Expired = true;
                stopwatch.Stop();
            }
        }
    }
}
