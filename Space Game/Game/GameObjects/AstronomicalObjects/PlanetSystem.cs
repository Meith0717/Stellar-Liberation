// PlanetSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.ItemManagement;
using CelestialOdyssey.Game.Layers;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;
using CelestialOdyssey.Game.Core.Utilitys;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    public enum Danger { None, Moderate, Medium, High }

    [Serializable]
    public class PlanetSystem : InteractiveObject
    {
        //--game object associated--------------------------------------------------------------//
        [JsonProperty] public Danger Danger { get; private set; }
        [JsonProperty] public CircleF SystemBounding { get; private set; }

        public PlanetSystem(Vector2 mapPosition, Vector2 sectorPosition, int boundaryRadius, string textureId, Color color, Danger danger)
            : base(mapPosition, textureId, 0.01f, 1)
        {
            TextureColor = color;
            Danger = danger;
            SystemBounding = new(sectorPosition, boundaryRadius);
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            LeftPressAction = () => {
                gameLayer.PopScene();
                Player.HyperDrive.TargetPlanetSystem = this;
            };

            base.Update(gameTime, inputState, gameLayer, scene);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this, IsHover);
            TextureManager.Instance.Draw(ContentRegistry.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, 0, TextureColor);
        }

        private Color GetColor() => Danger switch
        {
            Danger.None => Color.White,
            Danger.Moderate => Color.Yellow,
            Danger.Medium => Color.Orange,
            Danger.High => Color.Red,
            _ => throw new NotImplementedException()
        };

        //--------------------------------------------------------------------------------------//

        //--planetsystem scene associated-------------------------------------------------------//
        [JsonIgnore] public Player Player { get; private set; }
        [JsonProperty] public QuantumGate QuantumGate { get; private set; }
        [JsonProperty] public Star Star { get; private set; }
        [JsonProperty] public List<Planet> Planets { get; private set; }

        [JsonProperty] public readonly SpaceShipManager SpaceShipManager = new();
        [JsonIgnore] public readonly ProjectileManager ProjectileManager = new();
        [JsonIgnore] public readonly ItemManager ItemManager = new();

        //--generation associated--//
        public void SetObjects(Star star, List<Planet> planets, Player player, List<SpaceShip> spaceShips, QuantumGate quantumGate)
        {
            QuantumGate = quantumGate;
            Planets = planets;  
            Star = star;
            Player = player;
            var lst = new List<SpaceShip>();
            for (int i = 0; i < 30; i++) SpaceShipManager.Spawn(this, ExtendetRandom.NextVectorInCircle(SystemBounding), ShipType.EnemyFighter);
         }
        //------------------------//

        public void SpawnShip()
        {
            Player.Position = Vector2.Zero;
        }

        public void UpdateObjects(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            QuantumGate.Update(gameTime, inputState, gameLayer, scene);
            Star.Update(gameTime, inputState, gameLayer, scene);
            Player.Update(gameTime, inputState, gameLayer, scene);
            foreach (var item in Planets) item.Update(gameTime, inputState, gameLayer, scene);
            SpaceShipManager.Update(gameTime, inputState, gameLayer, scene);
            ItemManager.Update(gameTime, inputState, gameLayer, scene);
            ProjectileManager.Update(gameTime, inputState, gameLayer, scene);
        }

        //--------------------------------------------------------------------------------------//
    }
}
