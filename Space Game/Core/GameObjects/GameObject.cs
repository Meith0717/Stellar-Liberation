using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using System;
using Newtonsoft.Json;

namespace Galaxy_Explovive.Core.GameObject
{
    [Serializable]
    public abstract class GameObject
    {
        // Location
        [JsonProperty] public Vector2 Position { get; set; }
        [JsonProperty] public float Rotation { get; set; }

        // Rendering
        [JsonProperty] public Vector2 TextureOffset { get; set; }
        [JsonProperty] public string TextureId { get; set; }
        [JsonProperty] public float TextureScale { get; set; }
        [JsonProperty] public int TextureWidth { get; set; }
        [JsonProperty] public int TextureHeight { get; set; }
        [JsonProperty] public int TextureDepth { get; set; }
        [JsonProperty] public Color TextureColor { get; set; }
        [JsonProperty] public CircleF BoundedBox { get; set; }

        // Methods
        public abstract void UpdateLogik(GameTime gameTime, InputState inputState, GameEngine engine);
        public abstract void Draw(TextureManager textureManager, GameEngine engine);

    }
}
