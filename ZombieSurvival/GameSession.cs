using System;
using System.Drawing;
using System.Linq;
using WinFormsGameSDK;
using ZombieSurvival.Sprites;

namespace ZombieSurvival
{
    /// <summary>
    /// Represents a <see cref="ZombieSurvival"/> game session.
    /// </summary>
    sealed class GameSession : GameSessionBase
    {
        //private int zombieSpawnInterval = 2;
        //private int lastTime;

        /// <summary>
        /// Gets or sets whether to enable the team NPC.
        /// </summary>
        public bool TeamPlayerEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets whether to spawn zombies at a fixed interval.
        /// </summary>
        public bool ZombieSpawnerEnabled { get; set; }

        /// <summary>
        /// Gets the local player (controlled directly by a user).
        /// </summary>
        public PlayerSprite LocalPlayer
        {
            get { return SpriteMan.Players.First(p => p.IsLocal); }
        }

        private MoveDirection moveDirection = MoveDirection.None;
        /// <summary>
        /// Gets or sets the move direction of the local player.
        /// </summary>
        public MoveDirection MoveDirection
        {
            get { return moveDirection; }
            set
            {
                moveDirection = value;
                LocalPlayer.MoveDirection = value;
            }
        }

        private bool firing;
        /// <summary>
        /// Gets or sets whether the local player is firing.
        /// </summary>
        public bool Firing
        {
            get { return firing; }
            set
            {
                firing = value;
                LocalPlayer.Shooting = value;
            }
        }

        /// <summary>
        /// Gets the sprite manager for this game session.
        /// </summary>
        public MySpriteManager SpriteMan { get; } = new MySpriteManager();

        /// <summary>
        /// Gets the boundaries of the loaded map.
        /// </summary>
        public RectangleF MapBounds { get; } = new RectangleF(0, 0, 1000, 1000);

        public GameSession()
        {
            var truck = new TruckSprite();
            truck.Position = new PointF(200, 200);
            SpriteMan.Add(truck);

            var myPlayer = new PlayerSprite(100, 100, true);
            SpriteMan.Add(myPlayer);

            if (TeamPlayerEnabled)
            {
                var teamPlayer = new TeamPlayerSprite(myPlayer, SpriteMan.Zombies);
                SpriteMan.Add(teamPlayer);
            }

            foreach (var player in SpriteMan.Players)
                player.Update();

            // SpriteMan.Add(new BlockerSprite(200, 200, 200, 200));
            BuildBarrier();
        }

        /// <summary>
        /// Builds a barrier based on the configured map bounds.
        /// </summary>
        private void BuildBarrier()
        {
            var blockers = BlockerSprite.CreateRectangularEnclosure(MapBounds, 20);

            foreach (var blocker in blockers)
                SpriteMan.Add(blocker);
        }

        /// <summary>
        /// Specifies that the local player should use any device in its using range.
        /// </summary>
        public void Use()
        {
            var nearbyVehicle = LocalPlayer.NearbyVehicle;

            if (nearbyVehicle != null)
            {
                if (nearbyVehicle.Driver == null)
                {
                    nearbyVehicle.Driver = LocalPlayer;
                    LocalPlayer.Position = nearbyVehicle.GetDriverPosition();
                    LocalPlayer.ParentVehicle = nearbyVehicle;
                }
                else
                {
                    nearbyVehicle.Driver = null;
                    LocalPlayer.ParentVehicle = null;
                    LocalPlayer.Position = nearbyVehicle.GetDriverExitPoint();
                }
            }
        }

        /// <summary>
        /// Raises when half of a second has elapsed.
        /// </summary>
        protected override void OnSplitSecondElapsed()
        {
            foreach (var vehicle in SpriteMan.Vehicles)
            {
                PointF driverPos = vehicle.Position;

                if (
                    LocalPlayer.Vector.DistanceTo(driverPos) < 100)
                {
                    LocalPlayer.NearbyVehicle = vehicle;
                    return;
                }
                else
                {
                    LocalPlayer.NearbyVehicle = null;
                }
            }
        }

        /// <summary>
        /// Implements game logic.
        /// </summary>
        protected override void GameLoop()
        {
            foreach (var zombie in SpriteMan.Zombies)
            {
                zombie.Update();
            }

            foreach (var vehicle in SpriteMan.Vehicles)
                vehicle.Update();

            foreach (var contrail in SpriteMan.Contrails)
                contrail.Update();

            foreach (var player in SpriteMan.Players)
            {
                player.Update();
                ProcessBullets(player);
            }

            //ProcessUserInput();
            SpriteMan.RemoveExpired();
            //HitTestSpriteManager.Sprites();
        }

        /// <summary>
        /// Raises when a second has elapsed.
        /// </summary>
        protected override void OnSecondElapsed()
        {
            Random random = new Random();
            float x = random.Next(0, (int)MapBounds.Width);
            float y = random.Next(0, (int)MapBounds.Height);
            var pos = new PointF(x, y);

            if (SpriteMan.Zombies.Count < 30 && ZombieSpawnerEnabled)
            {
                var zombie = new ZombieSprite(SpriteMan.Players, pos);
                SpriteMan.Add(zombie);
                zombie.Update();
            }
        }

        //private void HitTestSpriteManager.Sprites()
        //{
        //    var blockingSpriteManager.Sprites = SpriteManager.Sprites.BlockingSpriteManager.Sprites;

        //    foreach (var spriteToTest in blockingSpriteManager.Sprites)
        //    {
        //        foreach (var sprite in blockingSpriteManager.Sprites)
        //        {
        //            if (sprite == spriteToTest) continue;

        //            bool colliding = spriteToTest.MovementCollision.CollidesWith(sprite.MovementCollision);
        //            Debug.WriteLine(colliding);

        //            if (colliding)
        //            {
        //                if (spriteToTest.Mass >= sprite.Mass && sprite.Mass != 0 || 
        //                    (spriteToTest.Mass == 0 && sprite.Mass != 0))
        //                {
        //                    float origFacingDegree = sprite.Vector.FacingDegree;
        //                    sprite.Vector.FaceTarget(spriteToTest.Vector);
        //                    sprite.Vector.Flip();
        //                    sprite.Vector.Project(3);
        //                    sprite.Vector.FacingDegree = origFacingDegree;
        //                    sprite.OnPositionChanged();
        //                }
        //            }
        //        }
        //    }
        //}

        //private void ProcessUserInput()
        //{
        //    Stack<MoveDirection> directions = new Stack<MoveDirection>();

        //    if (KeyInputManager.IsKeyPressed(Keys.W))
        //        directions.Push(MoveDirection.Forwards);

        //    if (KeyInputManager.IsKeyPressed(Keys.A))
        //        directions.Push(MoveDirection.Left);

        //    if (KeyInputManager.IsKeyPressed(Keys.S))
        //        directions.Push(MoveDirection.Backwards);

        //    if (KeyInputManager.IsKeyPressed(Keys.D))
        //        directions.Push(MoveDirection.Right);

        //    while (directions.Count > 0)
        //    {
        //        var D = directions.Pop();
        //        SpriteMan.Player.Move(D, FootMovementType);
        //    }
        //}

        /// <summary>
        /// Adds a blood contrail to the sprite manager based on the zombie and
        /// vector specified.
        /// </summary>
        /// <param name="zombie">The zombie that was shot.</param>
        /// <param name="vector">The vector to project through the back of the zombie.</param>
        private void AddBloodContrail(ZombieSprite zombie, Vector2D vector)
        {
            float travelDistance = 0;
            Vector2D startVector = vector.Clone();

            while (travelDistance < 500)
            {
                travelDistance++;
                startVector.Project(1);

                if (!zombie.ProjectileCollision.ContainsPoint(startVector.Position))
                {
                    var endVector = startVector.Clone();
                    endVector.Project(5);
                    var contrail = new ContrailSprite(startVector, endVector, ContrailKind.Blood);
                    contrail.BaseAnimationSpeed = 100;
                    SpriteMan.Add(contrail);
                    break;
                }
            }
        }

        /// <summary>
        /// Adds a bullet contrail to the sprite manager, for every traveling bullet associated
        /// with the specified player.
        /// </summary>
        private void ProcessBullets(PlayerSprite player)
        {
            while (player.BulletVectors.Count > 0)
            {
                var vector = player.BulletVectors.Pop();
                var endVector = vector.Clone();
                float travelDistance = 0;

                while (travelDistance < 1000)
                {
                    endVector.Project(1);
                    travelDistance++;

                    var hitSprite = SpriteManager.ProjectileBlocking.FirstOrDefault(s =>
                        s.ProjectileCollision.ContainsPoint(endVector.Position) && s != player);

                    if (hitSprite != null)
                    {
                        var zombie = hitSprite as ZombieSprite;
                        if (zombie != null)
                        {
                            zombie.Health -= 20;
                            AddBloodContrail(zombie, endVector);
                        }

                        SpriteMan.Add(new ContrailSprite(vector, endVector, ContrailKind.Bullet));
                        return;
                    }
                }
            }
        }

        //public void Shoot()
        //{
        //    var shootingVector = SpriteManager.Sprites.Player.Shoot();
        //    var endVector = shootingVector.Clone();
        //    float travelDistance = 0;

        //    while (travelDistance < 1000)
        //    {
        //        endVector.Project(1);
        //        travelDistance++;

        //        var hitSprite = SpriteManager.Sprites.Zombies.FirstOrDefault(z =>
        //            z.ProjectileCollision.ContainsPoint(endVector.Position));

        //        if (hitSprite != null)
        //        {
        //            hitSprite.Health -= 20;
        //            break;
        //        }
        //    }

        //    SpriteManager.Sprites.AddSprite(new ContrailSprite(shootingVector, endVector));
        //}
    }
}
