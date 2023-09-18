using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.Layers;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    [Serializable]
    public class Player : SpaceShip
    {
        [JsonIgnore] private readonly WeaponSystem mWeaponSystem;

        public Player(Vector2 position) : base(position, ContentRegistry.ship.Name, 50) 
        {
            mWeaponSystem = new(new(){ new(10000, 2200), new(10000, -2200), new(2000, 5000), new(2000, -5000)} );
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            if (!HyperDrive.IsActive)
            {
                if (inputState.HasMouseAction(MouseActionType.LeftClickHold)) mWeaponSystem.Fire(this);
                SublightEngine.FollowMouse(inputState, this, sceneLayer.WorldMousePosition);
            }

            mWeaponSystem.Update(gameTime, inputState, sceneLayer, this);

            base.Update(gameTime, inputState, sceneLayer);
            sceneLayer.Camera.SetPosition(Position);
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            TextureManager.Instance.DrawGameObject(this);
        }

        public void DrawPath(GameLayer gameLayer, MapLayer mapLayer)
        {
            var color = Color.LightGreen;
            var position = gameLayer.Map.GetMapPosition(Position);
            if (gameLayer.ActualPlanetSystem is null) TextureManager.Instance.Draw(ContentRegistry.dot, position, 0.005f, 0, 1, color);
            if (HyperDrive.TargetPosition is null) return;
            var target = gameLayer.Map.GetMapPosition((Vector2)HyperDrive.TargetPosition);
            TextureManager.Instance.DrawAdaptiveLine(position, target, color, 1, 0, mapLayer.Camera.Zoom);
        }
    }
}
