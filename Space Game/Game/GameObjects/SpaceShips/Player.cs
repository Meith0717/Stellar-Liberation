using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    [Serializable]
    public class Player : SpaceShip
    {

        public Player() : base(Vector2.Zero, ContentRegistry.player.Name, 40) 
        {
            WeaponSystem = new(new(){ new(1000, 5500), new(1000, -5500)}, 100 );
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            if (!HyperDrive.IsActive)
            {
                if (inputState.HasMouseAction(MouseActionType.LeftClickHold)) WeaponSystem.Fire(this, scene.WorldMousePosition);
                SublightEngine.FollowMouse(inputState, this, scene.WorldMousePosition);
            }

            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            scene.Camera.SetPosition(Position);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
