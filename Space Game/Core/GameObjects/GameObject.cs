using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.InputManagement;

namespace Galaxy_Explovive.Core.GameObject
{
    public abstract class GameObject
    {
        // Location
        [JsonProperty] public Vector2 Position { get; set; }
        [JsonProperty] public float Rotation { get; set; }

        // Rendering
        public Vector2 TextureOffset { get; set; }
        public string TextureId { get; set; }
        public float TextureSclae { get; set; }
        public int TextureWidth { get; set; }
        public int TextureHeight { get; set; }
        public int TextureDepth { get; set; }
        public Color TextureColor { get; set; }
        public CircleF BoundedBox { get; set; }

        public abstract void Update(GameTime gameTime, InputState inputState);
        public abstract void Draw();
    }
}
