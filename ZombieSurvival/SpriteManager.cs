using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinFormsGameSDK;
using WinFormsGameSDK.Sprites;
using ZombieSurvival.Sprites;

namespace ZombieSurvival
{
    /// <summary>
    /// Represents a sprite manager for the <see cref="ZombieSurvival"/> game.
    /// </summary>
    class MySpriteManager : SpriteManager
    {
        private readonly List<BlockerSprite> blockers = new List<BlockerSprite>();
        /// <summary>
        /// Gets all of the <see cref="BlockerSprite"/>s in the game.
        /// </summary>
        public IReadOnlyList<BlockerSprite> Blockers => blockers;

        private readonly List<ContrailSprite> contrails = new List<ContrailSprite>();
        /// <summary>
        /// Gets all of the <see cref="ContrailSprite"/>s in the game.
        /// </summary>
        public IReadOnlyList<ContrailSprite> Contrails => contrails;

        private readonly List<PlayerSprite> players = new List<PlayerSprite>();
        /// <summary>
        /// Gets all of the <see cref="PlayerSprite"/>s in the game.
        /// </summary>
        public IReadOnlyList<PlayerSprite> Players => players;

        private readonly List<ZombieSprite> zombies = new List<ZombieSprite>();
        /// <summary>
        /// Gets all of the <see cref="ZombieSprite"/>s in the game.
        /// </summary>
        public IReadOnlyList<ZombieSprite> Zombies => zombies;

        private readonly List<VehicleSprite> vehicles = new List<VehicleSprite>();
        /// <summary>
        /// Gets all of the <see cref="VehicleSprite"/>s in the game.
        /// </summary>
        public IReadOnlyList<VehicleSprite> Vehicles => vehicles;

        /// <summary>
        /// Removes all sprites that have expired.
        /// </summary>
        public override void RemoveExpired()
        {
            base.RemoveExpired();
            zombies.RemoveAll(s => s.Expired);
            contrails.RemoveAll(s => s.Expired);
        }

        /// <summary>
        /// Adds a sprite to the sprite manager.
        /// </summary>
        /// <param name="sprite">The sprite to add.</param>
        public override void Add(Sprite sprite)
        {
            base.Add(sprite);
            var vehicle = sprite as VehicleSprite;
            if (vehicle != null)
            {
                vehicles.Add(vehicle);
                return;
            }

            var zombie = sprite as ZombieSprite;
            if (zombie != null)
            {
                zombies.Add(zombie);
                return;
            }

            var contrail = sprite as ContrailSprite;
            if (contrail != null)
            {
                contrails.Add(contrail);
                return;
            }

            var player = sprite as PlayerSprite;
            if (player != null)
            {
                players.Add(player);
                return;
            }

            var blocker = sprite as BlockerSprite;
            if (blocker != null)
            {
                blockers.Add(blocker);
                return;
            }
        }

        /// <summary>
        /// Removes a sprite from the sprite manager.
        /// </summary>
        /// <param name="sprite">The sprite to remove.</param>
        public override void Remove(Sprite sprite)
        {
            base.Remove(sprite);
            var vehicle = sprite as VehicleSprite;
            if (vehicle != null)
            {
                vehicles.Remove(vehicle);
                return;
            }

            var player = sprite as PlayerSprite;
            if (player != null) players.Remove(player);

            var contrail = sprite as ContrailSprite;
            if (contrail != null) contrails.Remove(contrail);

            var zombie = sprite as ZombieSprite;
            if (zombie != null) zombies.Remove(zombie);

            var blocker = sprite as BlockerSprite;
            if (blocker != null) blockers.Remove(blocker);
        }
    }
}
