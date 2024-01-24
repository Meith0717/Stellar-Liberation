// Player.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Enemys;
using System;
using System.Linq;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Allies
{
    [Serializable]
    public class Player : AlliedShip
    {
        public Player() : base(Vector2.Zero, GameSpriteRegistries.destroyer, 1, new(20000), new(10f, 0.05f), new(300, new(120, 200, 240), 2, 2, 10000), new(100, 1000, 1), new(10000))
        {
            WeaponSystem.PlaceTurret(new(new(110, 35), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(110, -35), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-130, 100), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-130, -100), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-150, 0), 1, TextureDepth + 1));
        }

        public void SpawnInNewPlanetSystem(Vector2 position)
        {
            Position = position;
            Velocity = 0;
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            SublightEngine.ControlByInput(this, inputState, scene.WorldMousePosition);
            WeaponSystem.AimShip(SensorArray.GetAimingShip(Position));
            WeaponSystem.ControlByInput(inputState, scene.WorldMousePosition);
            foreach(EnemyShip enemy in SensorArray.OpponentsInRannge.Cast<EnemyShip>()) { enemy.IsSpotted = true; }
            base.Update(gameTime, inputState, scene);
        }
    }
}
