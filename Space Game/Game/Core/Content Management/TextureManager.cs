/*
 *  TextureManager.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using CelestialOdyssey.Game.Core.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;

namespace CelestialOdyssey.Core.GameEngine.Content_Management
{
    public sealed class TextureManager
    {
        private static TextureManager mInstance;

        public static TextureManager Instance
        {
            get
            {
                if (mInstance == null) { mInstance = new TextureManager(); }
                return mInstance;
            }
        }

        public float MaxLayerDepth = 10000f;

        private readonly Dictionary<string, Texture2D> mTextures = new();
        private readonly Dictionary<string, SpriteSheet> mSprites = new();
        private readonly Dictionary<string, SpriteFont> mSpriteFonts = new();
        public SpriteBatch SpriteBatch { get; private set; }

        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }

        public bool LoadTexture(ContentManager content, string id, string fileName)
        {
            Texture2D texture = content.Load<Texture2D>(fileName);
            mTextures.Add(id, texture);
            return true;
        }

        public bool LoadFont(ContentManager content, string id, string fileName)
        {
            SpriteFont spriteFont = content.Load<SpriteFont>(fileName);
            mSpriteFonts.Add(id, spriteFont);
            return true;
        }

        public bool LoadSprite(ContentManager content, string id, string fileName)
        {
             SpriteSheet spriteSheet = content.Load<SpriteSheet>($"{fileName}.sf", new JsonContentLoader());
            mSprites.Add(id, spriteSheet);
            return true;
        }



        public Texture2D GetTexture(string id)
        {
            Texture2D texture = mTextures[id];
            if (texture == null)
            {
                throw new Exception($"Error, Texture {id} was not found in TextureManager");
            }
            return texture;
        }
        public SpriteFont GetFont(string id)
        {
            SpriteFont spriteFont = mSpriteFonts[id];
            if (spriteFont == null)
            {
                throw new Exception($"Error, Font {id} was not found in TextureManager");
            }

            return spriteFont;
        }
        public SpriteSheet GetSprite(string id)
        {
            SpriteSheet spriteSheet = mSprites[id];
            if (spriteSheet == null)
            {
                throw new Exception($"Error, Font {id} was not found in TextureManager");
            }

            return spriteSheet;
        }

        private float GetDepth(int textureDepth)
        {
            var depth = textureDepth / MaxLayerDepth;
            if (depth > 1) throw new Exception();
            return depth;
        }

        // render Textures ___________________________________________________________________________

        public void Draw(string id, Vector2 position, float width, float height)
        {
            SpriteBatch.Draw(GetTexture(id), new RectangleF(position.X, position.Y, width, height).ToRectangle(), Color.White);
        }

        public void Draw(string id, Vector2 position, Vector2 offset, float sclae, float rotation, int depth, Color color)
        {
            SpriteBatch.Draw(GetTexture(id), position, null, color, rotation, offset, sclae, SpriteEffects.None, GetDepth(depth));
        }
        public void Draw(string id, Vector2 position, float sclae, float rotation, int depth, Color color)
        {
            Texture2D texture = GetTexture(id);
            Vector2 offset = new(texture.Width / 2, texture.Height / 2);
            SpriteBatch.Draw(GetTexture(id), position, null, color, rotation, offset, sclae, SpriteEffects.None, GetDepth(depth));
        }

        // render Game Objects ___________________________________________________________________________
        public void DrawGameObject(GameObject obj)
        {
            SpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, obj.TextureColor, obj.Rotation, obj.TextureOffset,
                obj.TextureScale, SpriteEffects.None, GetDepth(obj.TextureDepth));
        }
        public void DrawGameObject(GameObject obj, bool isHover)
        {
            var color = isHover ? Color.Gray : obj.TextureColor;
            SpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, color, obj.Rotation, obj.TextureOffset,
                obj.TextureScale, SpriteEffects.None, GetDepth(obj.TextureDepth));
        }

        // render String
        public void DrawString(string id, Vector2 position, string text, float scale, Color color)
        {
            SpriteBatch.DrawString(GetFont(id), text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
        }

        // render Circle ___________________________________________________________________________
        public void DrawCircle(Vector2 center, float radius, Color color, float thickness, int depth)
        {
            SpriteBatch.DrawCircle(center, radius, 90, color, thickness, GetDepth(depth));
        }

        public void DrawAdaptiveCircle(Vector2 center, float radius, Color color, float thickness, int depth, float zoom)
        {
            SpriteBatch.DrawCircle(center, radius, 90, color, thickness / zoom, GetDepth(depth));
        }

        // render Rectangle ___________________________________________________________________________
        public void DrawRectangleF(RectangleF rectangle, Color color, float thickness, int depth)
        {
            SpriteBatch.DrawRectangle(rectangle, color, thickness, GetDepth(depth));
        }

        public void DrawAdaptiveRectangleF(RectangleF rectangle, Color color, float thickness, int depth, float zoom)
        {
            SpriteBatch.DrawRectangle(rectangle, color, thickness / zoom, GetDepth(depth));
        }


        // render Line ___________________________________________________________________________
        public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness, int depth)
        {
            SpriteBatch.DrawLine(start, end, color, thickness, GetDepth(depth));
        }

        public void DrawLine(Vector2 start, float length, Color color, float thickness, int depth)
        {
            SpriteBatch.DrawLine(start, length, 0, color, thickness, GetDepth(depth));
        }


        public void DrawAdaptiveLine(Vector2 start, Vector2 end, Color color, float thickness, int depth, float zoom)
        {
            SpriteBatch.DrawLine(start, end, color, thickness / zoom, GetDepth(depth));
        }
    }
}