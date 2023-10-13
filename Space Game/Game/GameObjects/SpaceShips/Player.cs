// Player.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    [Serializable]
    public class Player : SpaceShip
    {

        public Player() : base(Vector2.Zero, ContentRegistry.player.Name, 1)
        {
            WeaponSystem = new(Color.LightBlue, 50, 50, 100);
            WeaponSystem.SetWeapon(new(110, 35));
            WeaponSystem.SetWeapon(new(110, -35));
            WeaponSystem.SetWeapon(new(-130, 100));
            WeaponSystem.SetWeapon(new(-130, -100));
            WeaponSystem.SetWeapon(new(-150, 0));
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            if (!HyperDrive.IsActive)
            {
                WeaponSystem.TargetPosition = scene.WorldMousePosition;
                inputState.DoMouseAction(MouseActionType.RightClickHold, () => WeaponSystem.Fire(ActualPlanetSystem.ProjectileManager, this));
                SublightEngine.FollowMouse(inputState, this, scene.WorldMousePosition);
            }

            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            scene.Camera.SetPosition(Position);
        }

        public override void HasCollide()
        {
            throw new NotImplementedException();
        }
    }
}
