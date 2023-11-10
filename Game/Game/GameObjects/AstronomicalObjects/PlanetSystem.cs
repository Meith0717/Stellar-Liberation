// PlanetSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.ItemManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Layers;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
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

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            LeftPressAction = () => {
                scene.GameLayer.PopScene();
                Player.HyperDrive.TargetPlanetSystem = this;
            };

            base.Update(gameTime, inputState, scene);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this, IsHover);
            TextureManager.Instance.Draw(TextureRegistries.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, 0, TextureColor);
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
            for (int i = 0; i < 10; i++) SpaceShipManager.Spawn(this, ExtendetRandom.NextVectorInCircle(SystemBounding), ShipType.EnemyBattleShip);
            for (int i = 0; i < 20; i++) SpaceShipManager.Spawn(this, ExtendetRandom.NextVectorInCircle(SystemBounding), ShipType.EnemyCorvette);
            for (int i = 0; i < 30; i++) SpaceShipManager.Spawn(this, ExtendetRandom.NextVectorInCircle(SystemBounding), ShipType.EnemyCarrior);

        }
        //------------------------//

        public void SpawnShip()
        {
            Player.Position = Vector2.Zero;
        }

        public void UpdateObjects(GameTime gameTime, InputState inputState, Scene scene)
        {
            QuantumGate.Update(gameTime, inputState, scene);
            Star.Update(gameTime, inputState, scene);
            Player.Update(gameTime, inputState, scene);
            foreach (var item in Planets) item.Update(gameTime, inputState, scene);
            SpaceShipManager.Update(gameTime, inputState, scene);
            ItemManager.Update(gameTime, inputState, scene);
            ProjectileManager.Update(gameTime, inputState, scene);
        }

        //--------------------------------------------------------------------------------------//
    }
}
