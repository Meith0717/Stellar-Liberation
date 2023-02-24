using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Space_Game.Core.InputManagement;
using Space_Game.Core.TextureManagement;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Space_Game.Core.GameObject
{
    public abstract class GameObject
    {
        // Location
        [JsonProperty] public Vector2 Position;
        [JsonProperty] public Vector2 TextureOffset;

        // Rendering
        [JsonProperty] public string TextureId;
        [JsonProperty] public float TextureSclae;
        [JsonProperty] public int TextureWidth;
        [JsonProperty] public int TextureHeight;
        [JsonProperty] public int TextureDepth;
        [JsonProperty] public float TextureRotation;
        [JsonProperty] public Color TextureColor;

        // Hover Stuff
        [JsonProperty] public bool Hover;
        [JsonProperty] public string NormalTextureId;
        [JsonProperty] public string HoverTectureId;

        // Update Logic
        public abstract void Update(GameTime gameTime, InputState inputState);
        // Draw Logic
        public abstract void Draw();
        public void DrawGameObject()
        {
            Texture2D sprite = TextureManager.GetInstance().GetTexture(TextureId);
            TextureManager.GetInstance().GetSpriteBatch().Draw(sprite, Position, null, TextureColor, 
                TextureRotation, TextureOffset, TextureSclae, SpriteEffects.None, TextureDepth);
        }
        // Other Stuff
        private void CheckForHover(InputState inputState, float radius)
        {
            Vector2 mousePosition = Globals.mCamera2d.ViewToWorld(inputState.mMousePosition.ToVector2());
            bool hover = (Vector2.Distance(mousePosition, Position) <= radius);
            TextureId = NormalTextureId;
            if (hover) { TextureId = HoverTectureId; }
            Hover = hover;
        }
        public void ClickOnObject(InputState inputState, float hoverRadius, Action leftClick, Action rightClick)
        {
            CheckForHover(inputState, hoverRadius);
            if (!Hover) { return; }
            if (inputState.mMouseActionType.Equals(MouseActionType.LeftClick) && leftClick != null)
            {
                leftClick();
                return;
            }
            if (inputState.mMouseActionType.Equals(MouseActionType.RightClick) && rightClick != null)
            {
                rightClick();
                return;
            }
        }
        public bool ViewIsSetTo(float zoom, int radius)
        {
            return IsInZoom(zoom) && IsInDistance(radius);
        }
        private bool IsInZoom(float zoom)
        {
            return Globals.mCamera2d.mZoom >= zoom;
        }
        private bool IsInDistance(int distance)
        {
            return Vector2.Distance(Position, Globals.mCamera2d.mPosition) < distance;
        }
    }
}
