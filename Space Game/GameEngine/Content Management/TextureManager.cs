/*
 *  TextureManager.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using CelestialOdyssey.Game.Core;
using CelestialOdyssey.GameEngine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Core.GameEngine.Content_Management
{
    public sealed class TextureManager
    {
        private static TextureManager mInstance;

        public static TextureManager Instance { 
            get { 
                if (mInstance == null) { mInstance = new TextureManager(); }
                return mInstance;
            } 
        }

        public float MaxLayerDepth = 10000f;

        private readonly Dictionary<string, Texture2D> mTextures = new();
        private readonly Dictionary<string, SpriteFont> mSpriteFonts = new();
        public SpriteBatch SpriteBatch { get; private set; }

        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
        }

        public bool LoadTexture(ContentManager content, string id, string fileName)
        {
            try
            {
                Texture2D texture = content.Load<Texture2D>(fileName);
                mTextures.Add(id, texture);
                return true;
            }
            catch 
            {
                throw new Exception($"Something is wrong with {id}, {fileName}");
            }
        }

        public void LoadSpriteTexture(ContentManager content, string id, string fileName)
        {
            try
            {
                SpriteFont spriteFont = content.Load<SpriteFont>(fileName);
                mSpriteFonts.Add(id, spriteFont);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something did'nt work when loading {id} Sprite: {ex}");
            }
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
        public SpriteFont GetSpriteFont(string id)
        {
            SpriteFont spriteFont = mSpriteFonts[id];
            if (spriteFont == null)
            {
                throw new Exception("Error, Texture was not found in TextureManager");
            }

            return spriteFont;
        }

        private float GetDepth(int textureDepth)
        {
            var depth = textureDepth / MaxLayerDepth;
            if (depth > 1) throw new Exception();
            return depth;
        }

        // render Textures ___________________________________________________________________________

        public void Draw(string id, Vector2 position, int width, int height)
        {
            SpriteBatch.Draw(GetTexture(id), new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
        }
        public void Draw(string id, Vector2 position, Vector2 offset, float sclae, float rotation, int depth, Color color)
        {
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
            var color = isHover ? Globals.HoverColor : obj.TextureColor;
            SpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, color, obj.Rotation, obj.TextureOffset,
                obj.TextureScale, SpriteEffects.None, GetDepth(obj.TextureDepth));
        }

        // render String
        public void DrawString(string id, Vector2 position, string text, float scale, Color color)
        {
            SpriteBatch.DrawString(GetSpriteFont(id), text, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
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