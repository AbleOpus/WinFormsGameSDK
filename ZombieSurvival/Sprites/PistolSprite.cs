namespace ZombieSurvival.Sprites
{
    /// <summary>
    /// Represents a pistol gun.
    /// </summary>
    public class PistolSprite : GunSprite
    {
        private const float PISTOL_LENGTH = 15;

        public PistolSprite()
        {
            Accuracy = 5;
            RoundsPerSecond = 5;
        }

        /// <summary>
        /// Update this sprite (typically called on every game loop iteration).
        /// </summary>
        public override void Update()
        {
            EndVector = Vector.Clone();
            EndVector.Project(PISTOL_LENGTH);
        }
    }
}
