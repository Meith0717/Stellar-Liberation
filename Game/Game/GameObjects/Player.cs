// Player.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.ItemManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement;
using System;

namespace StellarLiberation.Game.GameObjects
{
    [Serializable]
    public class Player : SpaceShip
    {

        public Inventory Inventory;

        public Player() : base(Vector2.Zero, TextureRegistries.player, 1, new(20000), new(10f, 0.01f), new(1000, new(0, 251, 255), 2, 2), new(100, 1000, 1), Factions.Enemys)
        {
            Inventory = new(20, 10000);

            WeaponSystem.PlaceTurret(new(new(110, 35), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(110, -35), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-130, 100), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-130, -100), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-150, 0), 1, TextureDepth + 1));

            mAi = new(new()
            {
                // new PatrollBehavior(),
                // new InterceptBehavior(),
                // new FarCombatBehavior(10000),
                // new FleeBehavior()
            })
            { Debug = true };
        }

        public void SpawnInNewPlanetSystem(Vector2 position)
        {
            Position = position;
            Velocity = 0;
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            WeaponSystem.StopFire();
            inputState.DoAction(ActionType.FireInitialWeapon, () => WeaponSystem.Fire());
            SublightEngine.FollowMouse(this, inputState, scene.WorldMousePosition);
            scene.Camera2D.SetPosition(Position);
            Inventory.Update(gameTime, this, scene);

            base.Update(gameTime, inputState, scene);
        }

        public override void Draw( Scene scene)
        {
            base.Draw(scene);
            Inventory.Draw(this);
        }
    }
}
