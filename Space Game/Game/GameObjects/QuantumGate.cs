// QuantumGate.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Layers;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects
{
    public class QuantumGate : InteractiveObject
    {

        private Vector2 mPlanetSystemPosition;

        public QuantumGate(Vector2 position, Vector2 planetSystemPosition)
            : base(position, ContentRegistry.placeHolder, 10, 50) 
        {
            mPlanetSystemPosition = planetSystemPosition;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            LeftPressAction = () => { 
                gameLayer.LoadMap(mPlanetSystemPosition);
            };
            RemoveFromSpatialHashing(scene);
            base.Update(gameTime, inputState, gameLayer, scene);
            AddToSpatialHashing(scene);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this, IsHover);
        }
    }
}
