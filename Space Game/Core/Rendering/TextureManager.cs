using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections;

namespace Galaxy_Explovive.Core.TextureManagement
{
    public class TextureManager
    {
        const int MaxLayerDepth = 10000;

        // Content Tables
        private readonly Hashtable mTextures = new();
        private readonly Hashtable mSpriteFonts = new();

        // Sprite Batch
        public SpriteBatch SpriteBatch { get; private set; }

        public TextureManager(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }

        public void LoadTexture(ContentManager content, string id, string fileName)
        {
            mTextures.Add(id, content.Load<Texture2D>(fileName));
        }

        public void LoadSpriteTexture(ContentManager content, string id, string fileName)
        {
            mSpriteFonts.Add(id, content.Load<SpriteFont>(fileName));
        }

        public Texture2D GetTexture(string id)
        {
            Texture2D texture = (Texture2D)mTextures[id];
            if (texture == null)
            {
                throw new System.Exception($"Error, Texture {id} was not found!");
            }
            return texture;
        }
        public SpriteFont GetSpriteFont(string id)
        {
            SpriteFont spriteFont = (SpriteFont)mSpriteFonts[id];
            if (spriteFont == null)
            {
                throw new System.Exception("Error, Texture was not found!");
            }

            return spriteFont;
        }

        // render Textures ___________________________________________________________________________

        public void Draw(string id, Vector2 position, int width, int height)
        {
            SpriteBatch.Draw(GetTexture(id), new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
        }
        public void Draw(string id, Vector2 position, Vector2 offset, float sclae, float rotation, int? depth, Color color)
        {
            if (depth.HasValue)
            {
                SpriteBatch.Draw(GetTexture(id), position, null, color, rotation, offset, sclae, SpriteEffects.None, depth.Value / MaxLayerDepth);
                return;
            }
            SpriteBatch.Draw(GetTexture(id), position, null, color, rotation, offset, sclae, SpriteEffects.None, MaxLayerDepth);
        }

        // render Game Objects ___________________________________________________________________________
        public void DrawGameObject(GameObject.GameObject obj)
        {
            if (obj.TextureDepth.HasValue)
            {
                SpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, obj.TextureColor, obj.Rotation, obj.TextureOffset,
                    obj.TextureScale, SpriteEffects.None, obj.TextureDepth.Value / MaxLayerDepth);
                return;
            }
            SpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, obj.TextureColor, obj.Rotation, obj.TextureOffset,
            obj.TextureScale, SpriteEffects.None, MaxLayerDepth);

        }
        public void DrawGameObject(GameObject.GameObject obj, bool isHover)
        {
            var color = isHover ? Globals.HoverColor : obj.TextureColor;
            if (obj.TextureDepth.HasValue)
            {
                SpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, color, obj.Rotation, obj.TextureOffset,
                obj.TextureScale, SpriteEffects.None, obj.TextureDepth.Value / MaxLayerDepth);
                return;
            }
            SpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, color, obj.Rotation, obj.TextureOffset,
            obj.TextureScale, SpriteEffects.None, MaxLayerDepth);
        }

        // render String
        public void DrawString(string id, Vector2 position, string text, float scale, Color color)
        {
            SpriteBatch.DrawString(GetSpriteFont(id), text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
        }

        // render Circle ___________________________________________________________________________
        public void DrawCircle(Vector2 center, float radius, Color color, float thickness, int? depth = null)
        {
            if (depth.HasValue)
            {
                SpriteBatch.DrawCircle(center, radius, 90, color, thickness, depth.Value / MaxLayerDepth);
                return;
            }
            SpriteBatch.DrawCircle(center, radius, 90, color, thickness, MaxLayerDepth);
        }

        // render Rectangle ___________________________________________________________________________
        public void DrawRectangleF(RectangleF rectangle, Color color, float thickness, int? depth = null)
        {
            if (depth.HasValue)
            {
                SpriteBatch.DrawRectangle(rectangle, color, thickness, depth.Value / MaxLayerDepth);
                return;
            }
            SpriteBatch.DrawRectangle(rectangle, color, thickness, MaxLayerDepth);
        }

        // render Line ___________________________________________________________________________
        public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness, int? depth = null)
        {
            if (depth.HasValue)
            {
                SpriteBatch.DrawLine(start, end, color, thickness, depth.Value / MaxLayerDepth);
                return;
            }
            SpriteBatch.DrawLine(start, end, color, thickness, MaxLayerDepth);
        }
    }
}