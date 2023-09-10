using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.Inventory;
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
        [JsonProperty] public CargoHold Inventory { get; private set; }
        [JsonIgnore] private readonly Weapon mWeapon;
        [JsonIgnore] private readonly Weapon mWeapon1;
        [JsonIgnore] private readonly Weapon mWeapon2;
        [JsonIgnore] private readonly Weapon mWeapon3;
        [JsonIgnore] private readonly SublightEngine mSublightEngine;
        [JsonIgnore] private readonly HyperDrive mHyperdrive;

        public Player(Vector2 position) : base(position, ContentRegistry.ship.Name, 50) 
        { 
            Inventory = new(16);
            mWeapon = new(new(10000, 2200));
            mWeapon1 = new(new(10000, -2200));
            mWeapon2 = new(new(2000, 5000));
            mWeapon3 = new(new(2000, -5000));
            mHyperdrive = new(2500, 100);
            mSublightEngine = new(50);
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {

            System.Diagnostics.Debug.WriteLine(mHyperdrive.ActualCharging);

            if (inputState.HasMouseAction(MouseActionType.LeftClickHold))
            {
                if (!mHyperdrive.IsActive)
                {
                    mWeapon.Fire(this);
                    mWeapon1.Fire(this);
                    mWeapon2.Fire(this);
                    mWeapon3.Fire(this);
                }
            }

            mHyperdrive.Update(gameTime, this);
            if (!mHyperdrive.IsActive) mSublightEngine.Update(inputState, this, sceneLayer.WorldMousePosition);

            mWeapon.Update(gameTime, inputState, sceneLayer, this);
            mWeapon1.Update(gameTime, inputState, sceneLayer, this);
            mWeapon2.Update(gameTime, inputState, sceneLayer, this);
            mWeapon3.Update(gameTime, inputState, sceneLayer, this);
            CollectItems(sceneLayer);

            base.Update(gameTime, inputState, sceneLayer);
            sceneLayer.Camera.SetPosition(Position);
        }

        public void SetTarget(PlanetSystem planetSystem, MapLayer mapLayer)
        {
            mHyperdrive.SetTarget(planetSystem, this, mapLayer);
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
            if (mHyperdrive.TargetPosition is null) return;
            var target = gameLayer.Map.GetMapPosition((Vector2)mHyperdrive.TargetPosition);
            TextureManager.Instance.DrawAdaptiveLine(position, target, color, 1, 0, mapLayer.Camera.Zoom);
        }

        private void CollectItems(SceneLayer sceneLayer)
        {
            var objects = sceneLayer.GetObjectsInRadius<Item>(Position, 500);
            foreach (var item in objects)
            {
                if (!BoundedBox.Contains(item.Position) || !Inventory.AddItem(item)) continue;
                item.Collect();
            }
        }
    }
}
