using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    public class Star : GameObject
    {
        [JsonProperty]
        private Color mStarColor;

        public Star(Vector2 position, string textureId, float textureScale, Color starColor) 
            : base(position, textureId, textureScale, 2)
        {
            mStarColor = starColor;
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            RemoveFromSpatialHashing(sceneLayer);
            base.Update(gameTime, inputState, sceneLayer);
            if (mStarColor != Color.Transparent)
            {
                Rotation += 0.001f;
            }
            AddToSpatialHashing(sceneLayer);
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw(ContentRegistry.starLightAlpha.Name, Position, TextureOffset, TextureScale * 2f, Rotation, 3, mStarColor);
        }
    }
}
