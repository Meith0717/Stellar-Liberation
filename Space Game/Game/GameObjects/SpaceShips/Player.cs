﻿using CelestialOdyssey.Core.GameEngine.Content_Management;
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
        [JsonProperty] private Weapon mWeapon = new Weapon(new(2000, 0));
        [JsonIgnore] private SublightEngine mSublightEngine;
        [JsonIgnore] private HyperDrive mHyperdrive;

        public Player(Vector2 position) : base(position, ContentRegistry.ship.Name, 10) 
        { 
            Inventory = new(16);
            mHyperdrive = new(1000, 2000);
            mSublightEngine = new(50);
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            if (inputState.HasMouseAction(MouseActionType.LeftClickHold))
            {
                if (!mHyperdrive.IsActive)
                {
                    mWeapon.Fire(this);
                }
            }

            mHyperdrive.Update(gameTime, this);
            if (!mHyperdrive.IsActive) mSublightEngine.Update(inputState, this, sceneLayer.WorldMousePosition);

            mWeapon.Update(gameTime, inputState, sceneLayer, this);
            CollectItems(sceneLayer);

            base.Update(gameTime, inputState, sceneLayer);
            sceneLayer.Camera.SetPosition(Position);
        }

        public void SetTarget(PlanetSystem planetSystem)
        {
            mHyperdrive.SetTarget(planetSystem, this);
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
