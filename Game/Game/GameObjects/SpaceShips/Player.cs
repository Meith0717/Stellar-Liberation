// Player.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.ItemManagement;
using System;

namespace StellarLiberation.Game.GameObjects.SpaceShipManagement
{
    [Serializable]
    public class Player : SpaceShip
    {

        public Inventory Inventory;

        public Player() : base(Vector2.Zero, TextureRegistries.player, 1, new(20000), new(10f, 0.05f), new(1000, Color.LightBlue, 2, 2, 10000), new(100, 1000, 1), Factions.Allies)
        {
            Inventory = new(500, 10000);

            WeaponSystem.PlaceTurret(new(new(110, 35), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(110, -35), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-130, 100), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-130, -100), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-150, 0), 1, TextureDepth + 1));

            mAi = new(new()
            {
                //new PatrollBehavior(),
                //new InterceptBehavior(),
                //new FarCombatBehavior(),
                //new FleeBehavior()
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
            SublightEngine.ControlByInput(this, inputState, scene.WorldMousePosition);
            inputState.DoAction(ActionType.RightClickHold, () => WeaponSystem.AimPosition(scene.WorldMousePosition));
            inputState.DoAction(ActionType.LeftClickHold, () => WeaponSystem.Fire());
            Inventory.Update(gameTime, this, scene);

            base.Update(gameTime, inputState, scene);
            WeaponSystem.AimShip(SensorArray.GetAimingShip(Position, Fraction));
            System.Diagnostics.Debug.WriteLine(Position);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            Inventory.Draw(this);
        }
    }
}
