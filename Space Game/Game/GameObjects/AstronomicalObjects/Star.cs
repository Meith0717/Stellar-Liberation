using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    public class Star : GameObject
    {
        [JsonProperty] public Color StarColor { get; private set; }


        public Star(Vector2 position, string textureId, float textureScale, Color starColor) 
            : base(position, textureId, textureScale, 2)
        {
            StarColor = starColor;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            RemoveFromSpatialHashing();
            base.Update(gameTime, inputState);
            if (StarColor != Color.Transparent)
            {
                Rotation += 0.001f;
            }
            AddToSpatialHashing();
        }

        public override void Draw()
        {
            base.Draw();
            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw(ContentRegistry.starLightAlpha.Name, Position, TextureOffset, TextureScale * 1.5f, Rotation, TextureDepth - 1, StarColor);
        }
    }
}
