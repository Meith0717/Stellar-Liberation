// PlanetSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Enemys;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    [Serializable]
    public class PlanetSystem : GameObject2D
    {
        [JsonIgnore] public GameObjectManager AstronomicalObjsManager { get; private set; }
        [JsonIgnore] private PlanetSystemInstance mInstance;

        [JsonProperty] public readonly GameObjectManager GameObjectManager;
        [JsonProperty] private readonly int mSeed;

        public PlanetSystem(Vector2 position, int seed) : base(position, GameSpriteRegistries.star, .01f, 1)
        {
            mSeed = seed;
            GameObjectManager = new();

            for (int i = 0; i < 5; i++) GameObjectManager.AddObj(EnemyFactory.Get(EnemyId.EnemyBattleShip, ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 100000))));
            for (int i = 0; i < 4; i++) GameObjectManager.AddObj(EnemyFactory.Get(EnemyId.EnemyBomber, ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 100000))));
             for (int i = 0; i < 1; i++) GameObjectManager.AddObj(EnemyFactory.Get(EnemyId.EnemyCarrior, ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 100000))));
        }

        public PlanetSystemInstance GetInstance()
        {
            if (mInstance is not null) return mInstance;

            var seededRandom = new Random(mSeed);
            AstronomicalObjsManager = new GameObjectManager();

            var star = StarGenerator.Generat(seededRandom);
            TextureColor = star.TextureColor;

            AstronomicalObjsManager.AddObj(star);
            var distanceToStar = (int)star.BoundedBox.Radius;
            var amount = Triangular.Sample(seededRandom, 1, 10, 7);
            for (int i = 1; i <= amount; i++)
            {
                distanceToStar += seededRandom.Next(40000, 80000);
                AstronomicalObjsManager.AddObj(PlanetGenerator.GetPlanet(seededRandom, star.Kelvin, distanceToStar));
            }
            distanceToStar += 50000;

            AstronomicalObjsManager.AddRange(AsteroidGenerator.GetAsteroidsRing(Position, distanceToStar));
            mInstance = new(GameObjectManager, AstronomicalObjsManager, star);
            return mInstance;
        }

        public void ClearInstance() => mInstance = null;

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            void LeftPressAction()
            {
                scene.GameLayer.HudLayer.Hide = false;
                scene.GameLayer.PopScene();
                scene.GameLayer.Player.HyperDrive.SetTarget(this);
            };

            base.Update(gameTime, inputState, scene);
            GameObject2DInteractionManager.Manage(inputState, this, scene, LeftPressAction, null, null);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw(GameSpriteRegistries.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, 0, TextureColor);
        }

    }
}
