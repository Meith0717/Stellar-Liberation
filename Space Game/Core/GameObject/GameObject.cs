using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Space_Game.Core.InputManagement;

namespace Space_Game.Core.GameObject
{
    public abstract class GameObject
    {
        // Location
        [JsonProperty] public Vector2 Position { get; set; }
        [JsonProperty] public float Rotation { get; set; }

        // Rendering
        public Vector2 TextureOffset { get; set; }
        public string NormalTextureId { get; set; }
        public float TextureSclae { get; set; }
        public int TextureWidth { get; set; }
        public int TextureHeight { get; set; }
        public int TextureDepth { get; set; }
        public Color TextureColor { get; set; }

        public abstract void Update(GameTime gameTime, InputState inputState);
        public abstract void Draw();
    }
}
