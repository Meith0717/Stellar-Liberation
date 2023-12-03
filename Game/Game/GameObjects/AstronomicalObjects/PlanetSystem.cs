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
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Core.Utilitys;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    public enum Danger { None, Moderate, Medium, High }

    [Serializable]
    public class PlanetSystem : GameObject2D
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

            for (int i = 0; i < 80; i++) mAsteroids.Add(new(ExtendetRandom.NextVectorInCircle(SystemBounding)));
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            var LeftPressAction = () => {
                scene.GameLayer.HudLayer.Hide = false;
                scene.GameLayer.PopScene();
                scene.GameLayer.Player.HyperDrive.SetTarget(this);
            };

            GameObject2DInteractionManager.Manage(inputState, this, scene, LeftPressAction, null);

            base.Update(gameTime, inputState, scene);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw(TextureRegistries.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, 0, TextureColor);
        }

        //--------------------------------------------------------------------------------------//

        //--planetsystem scene associated-------------------------------------------------------//
        [JsonProperty] public Star Star { get; private set; }
        [JsonProperty] public List<Planet> Planets { get; private set; }

        [JsonProperty] public readonly SpaceShipManager SpaceShipManager = new();

        [JsonProperty] public readonly List<Asteroid> mAsteroids = new();

        //--generation associated--//
        public void SetObjects(Star star, List<Planet> planets)
        {
            Planets = planets;  
            Star = star;
            for (int i = 0; i < 10; i++) SpaceShipManager.Spawn(ExtendetRandom.NextVectorInCircle(SystemBounding), ShipType.EnemyBattleShip);
            for (int i = 0; i < 10; i++) SpaceShipManager.Spawn(ExtendetRandom.NextVectorInCircle(SystemBounding), ShipType.EnemyCorvette);
            for (int i = 0; i < 3; i++) SpaceShipManager.Spawn(ExtendetRandom.NextVectorInCircle(SystemBounding), ShipType.EnemyCarrior);

        }
        //------------------------//


        public void UpdateObjects(GameTime gameTime, InputState inputState, Scene scene)
        {
            Star.Update(gameTime, inputState, scene);
            foreach (var item in Planets) item.Update(gameTime, inputState, scene);
            SpaceShipManager.Update(gameTime, inputState, scene);
            foreach (var asteroid in mAsteroids) asteroid.Update(gameTime, inputState, scene);
        }

        //--------------------------------------------------------------------------------------//
    }
}
