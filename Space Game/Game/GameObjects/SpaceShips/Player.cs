using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;
using CelestialOdyssey.Game.Core;
using MonoGame.Extended;
using CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    [Serializable]
    public class Player : SpaceShip
    {

        public Player() : base(Vector2.Zero, ContentRegistry.player.Name, 1) 
        {
            WeaponSystem = new(Color.LightBlue, 100, 100, 500);
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
                inputState.DoMouseAction(MouseActionType.LeftClickHold, () => WeaponSystem.Fire(ActualPlanetSystem.ProjectileManager, this));
                SublightEngine.FollowMouse(inputState, this, scene.WorldMousePosition);
            }

            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            scene.Camera.SetPosition(Position);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
            //DefenseSystem.DrawLive(this);
            sceneManagerLayer.DebugSystem.DrawSensorRadius(Position, 2000000, scene);
        }
    }
}
