using WinFormsGameSDK.Sprites;

namespace ZombieSurvival.Sprites
{
    /// <summary>
    /// Represents a firearm.
    /// </summary>
    public abstract class GunSprite : LineSprite
    {
        /// <summary>
        /// Gets the accuracy of the gun.
        /// </summary>
        public float Accuracy { get; protected set; }

        /// <summary>
        /// Gets the clip capacity of the gun.
        /// </summary>
        public int ClipCapacity { get; protected set; }

        /// <summary>
        /// Gets how many rounds are loaded into the gun.
        /// </summary>
        public int LoadedRounds { get; protected set; }

        /// <summary>
        /// Gets how many rounds per second the gun can fire.
        /// </summary>
        public int RoundsPerSecond { get; protected set; }

        /// <summary>
        /// Gets the shoot interval in milliseconds.
        /// </summary>
        public int ShootInterval => 1000/RoundsPerSecond;

        /// <summary>
        /// Reloads the clip of the gun.
        /// </summary>
        public void Reload()
        {
            LoadedRounds = ClipCapacity;
        }
    }
}
