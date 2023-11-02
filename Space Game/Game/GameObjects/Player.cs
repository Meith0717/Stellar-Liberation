// Player.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.AI.Behaviors;
using CelestialOdyssey.Game.Core.AI.Behaviors.Combat;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.ItemManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Layers;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;
using System.Security.Cryptography.X509Certificates;

namespace CelestialOdyssey.Game.GameObjects
{
    [Serializable]
    public class Player : SpaceShip
    {

        private Inventory mInventory;

        public Player() : base(Vector2.Zero, ContentRegistry.player.Name, 1, new(50000), new(1f, 0.1f), new(200, Color.BlueViolet, 2, 2), new(10000000, 10000000, 1, 1), Factions.Enemys)
        {
            mInventory = new();

            WeaponSystem.PlaceTurret(new(new(110, 35), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(110, -35), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-130, 100), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-130, -100), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-150, 0), 1, TextureDepth + 1));

            mAi = new(new()
            {
                new PatrollBehavior(),
                new FarCombatBehavior(50000),
                new FleeBehavior(),
                new InterceptBehavior(),
            })
            { Debug = true };
        }

        public void SpawnInNewPlanetSystem(Vector2 position)
        {
            Position = position;
            Velocity = 0;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            // WeaponSystem.StopFire();
            // inputState.DoMouseAction(MouseActionType.RightClickHold, () => WeaponSystem.Fire());
            // SublightEngine.FollowMouse(inputState, this, scene.WorldMousePosition);
            mInventory.Update(this, scene);

            base.Update(gameTime, inputState, gameLayer, scene);
            scene.Camera.SetPosition(Position);
        }

        public override void HasCollide()
        {
            throw new NotImplementedException();
        }
    }
}
