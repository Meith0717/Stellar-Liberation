// PlanetSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.ItemManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    public enum Danger { None, Moderate, Medium, High }

    [Serializable]
    public class PlanetSystem : InteractiveObject
    {
        // Game Objects
        [JsonIgnore] public Player Player { get; private set; }
        [JsonProperty] public Star Star { get; private set; }
        [JsonProperty] public List<Planet> Planets { get; private set; }

        // Other Stuff
        [JsonProperty] public readonly SpaceShipManager SpaceShipManager = new();
        [JsonIgnore] public readonly ProjectileManager ProjectileManager = new();
        [JsonIgnore] public readonly ItemManager ItemManager = new();

        // Atributes
        [JsonProperty] public Danger Danger { get; private set; }
        [JsonProperty] public CircleF SystemBounding { get; private set; }

        public PlanetSystem(Vector2 mapPosition, Vector2 sectorPosition, int boundaryRadius, string textureId, Color color, Danger danger)
            : base(mapPosition, textureId, 0.01f, 1)
        {
            TextureColor = color;
            Danger = danger;
            SystemBounding = new(sectorPosition, boundaryRadius);
        }

        public void SetObjects(Star star, List<Planet> planets, Player player, List<SpaceShip> spaceShips)
        {
            Planets = planets;
            Star = star;
            Player = player;
            SpaceShipManager.AddRange(spaceShips);
        }

        [JsonIgnore] private Color mCrosshairColor;

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            mCrosshairColor = IsHover ? Color.MonoGameOrange : GetColor();
        }

        private Color GetColor() => Danger switch
        {
            Danger.None => Color.White,
            Danger.Moderate => Color.Yellow,
            Danger.Medium => Color.Orange,
            Danger.High => Color.Red,
            _ => throw new NotImplementedException()
        };

        public void UpdateObjects(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            Star.Update(gameTime, inputState, sceneManagerLayer, scene);
            Player.Update(gameTime, inputState, sceneManagerLayer, scene);
            foreach (var item in Planets) item.Update(gameTime, inputState, sceneManagerLayer, scene);
            SpaceShipManager.Update(gameTime, inputState, sceneManagerLayer, scene);
            ItemManager.Update(gameTime, inputState, sceneManagerLayer, scene);
            ProjectileManager.Update(gameTime, inputState, sceneManagerLayer, scene);
        }

        public void DrawOrbits(Scene scene)
        {
            foreach (var item in Planets)
            {
                TextureManager.Instance.DrawAdaptiveCircle(Star.Position, item.OrbitRadius, new(10, 10, 10, 10), 1, item.TextureDepth - 1, scene.Camera.Zoom);
            }
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this, IsHover);
            TextureManager.Instance.Draw(ContentRegistry.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, 0, TextureColor);
            TextureManager.Instance.Draw(ContentRegistry.mapCrosshair, Position, TextureScale * 2, Rotation, 1, mCrosshairColor);
        }

        public override void LeftPressAction() {; }

        public override void RightPressAction() {; }
    }
}
