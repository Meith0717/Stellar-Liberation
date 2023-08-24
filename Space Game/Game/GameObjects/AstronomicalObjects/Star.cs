using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
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

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            RemoveFromSpatialHashing(engine);
            base.Update(gameTime, inputState, engine);
            if (mStarColor != Color.Transparent)
            {
                Rotation += 0.001f;
            }
            AddToSpatialHashing(engine);
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw(ContentRegistry.starLightAlpha.Name, Position, TextureOffset, TextureScale * 2f, Rotation, 3, mStarColor);
        }
    }
}
