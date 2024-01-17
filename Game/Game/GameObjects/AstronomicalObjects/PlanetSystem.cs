// PlanetSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    [Serializable]
    public class PlanetSystem : GameObject2D
    {
        [JsonProperty] public readonly GameObjectManager GameObjectManager;
        [JsonProperty] private readonly int mSeed;
        [JsonIgnore] private PlanetSystemInstance mInstance;

        public PlanetSystem(Vector2 position, int seed) : base(position, GameSpriteRegistries.star, .01f, 1)
        {
            mSeed = seed;
            GameObjectManager = new();
        }

        public PlanetSystemInstance GetInstance()
        {
            if (mInstance is not null) return mInstance;

            var seededRandom = new Random(mSeed);
            var astronomicalObjsManager = new GameObjectManager();

            var star = StarGenerator.Generat(seededRandom);
            TextureColor = star.TextureColor;


            astronomicalObjsManager.AddObj(star);
            astronomicalObjsManager.AddRange(AsteroidGenerator.GetAsteroidsRing(Position, 100000));

            mInstance = new(GameObjectManager, astronomicalObjsManager);
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
