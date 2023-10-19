// Player.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.ItemManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.GameObjects
{
    [Serializable]
    public class Player : SpaceShip
    {

        private Inventory mInventory;

        public Player() : base(Vector2.Zero, ContentRegistry.player.Name, 1, new(10000, 10000), new(0.5f, 0.01f), new(1, 1), new(Color.BlueViolet, 15, 15, 50, 5000), new(100000, 100000, 1, 1))
        {
            mInventory = new();
            WeaponSystem.SetWeapon(new(110, 35));
            WeaponSystem.SetWeapon(new(110, -35));
            WeaponSystem.SetWeapon(new(-130, 100));
            WeaponSystem.SetWeapon(new(-130, -100));
            WeaponSystem.SetWeapon(new(-150, 0));

            mAi = new(new());
        }

        public void SpawnInNewPlanetSystem(Vector2 position)
        {
            Position = position;
            SublightEngine.SetTarget(this, null);
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {

            WeaponSystem.VectorTarget = scene.WorldMousePosition;
            inputState.DoMouseAction(MouseActionType.RightClickHold, () => WeaponSystem.Fire());
            inputState.DoMouseAction(MouseActionType.LeftClick, () => SublightEngine.SetTarget(this, scene.WorldMousePosition));
            mInventory.Update(this, scene);

            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            scene.Camera.MoveToTarget(Position);
        }

        public override void HasCollide()
        {
            throw new NotImplementedException();
        }
    }
}
