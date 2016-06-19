using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsGameSDK.Sprites;

namespace WinFormsGameSDK
{
    /// <summary>
    /// Manages all game sprites.
    /// </summary>
    public class SpriteManager
    {
        private readonly List<Sprite> all = new List<Sprite>();
        /// <summary>
        /// Gets all of the sprites in the game.
        /// </summary>
        public IEnumerable<Sprite> All => all;

        private static readonly List<CollidableSprite> movementBlocking = new List<CollidableSprite>();
        /// <summary>
        /// Gets all of the movement-collidable sprites in the game.
        /// </summary>
        public static IEnumerable<CollidableSprite> MovementBlocking => movementBlocking;

        private static readonly List<CollidableSprite> projectileBlocking = new List<CollidableSprite>();
        /// <summary>
        /// Gets all of the projectile-collidable sprites in the game.
        /// </summary>
        public static IEnumerable<CollidableSprite> ProjectileBlocking => projectileBlocking;

        /// <summary>
        /// Gets the total amount of sprites loaded into the game.
        /// </summary>
        public int SpriteCount => all.Count;

        /// <summary>
        /// Adds a sprite to the sprite manager.
        /// </summary>
        /// <param name="sprite">The sprite to add.</param>
        public virtual void Add(Sprite sprite)
        {
            all.Add(sprite);
#if DEBUG
            if (String.IsNullOrEmpty(sprite.ID))
                NameSprite(sprite);
#endif

            var collidable = sprite as CollidableSprite;
            if (collidable != null)
            {
                if (collidable.MovementCollision != null)
                    movementBlocking.Add(collidable);

                if (collidable.ProjectileCollision != null)
                    projectileBlocking.Add(collidable);
            }
        }

        /// <summary>
        /// Removes a sprite from the sprite manager.
        /// </summary>
        /// <param name="sprite">The sprite to remove.</param>
        public virtual void Remove(Sprite sprite)
        {
            all.Remove(sprite);

            var collidable = sprite as CollidableSprite;
            if (collidable != null)
            {
                movementBlocking.Remove(collidable);
                projectileBlocking.Remove(collidable);
            }
        }

        /// <summary>
        /// Automatically names sprites.
        /// </summary>
        protected virtual void NameSprite(Sprite sprite)
        {
            string baseName = sprite.GetType().Name;
            int num = 1;
            while (all.Any(s => s.ID == baseName + num)) num++;
            sprite.ID = baseName + num;
        }

        /// <summary>
        /// Removes all sprites that have expired.
        /// </summary>
        public virtual void RemoveExpired()
        {
            all.RemoveAll(s => s.Expired);
            projectileBlocking.RemoveAll(s => s.Expired);
            movementBlocking.RemoveAll(s => s.Expired);
        }
    }
}
