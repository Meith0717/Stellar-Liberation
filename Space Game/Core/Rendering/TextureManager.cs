using Galaxy_Explovive.Core.GameObject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections;
using System.Diagnostics;

namespace Galaxy_Explovive.Core.TextureManagement;

public class TextureManager
{
    private static TextureManager sInstance;

    private SpriteBatch mSpriteBatch;
    private ContentManager mContentManager;

    readonly Hashtable mTextures = new();
    readonly Hashtable mSpriteFonts = new();

    private const int MaxLayerHeight = 10000;
    public static TextureManager Instance { get { return sInstance ??= new TextureManager(); } }

    public SpriteBatch GetSpriteBatch()
    {
        return mSpriteBatch;
    }

    public void SetSpriteBatch(SpriteBatch spriteBatch)
    {
        mSpriteBatch = spriteBatch;
    }
    public void SetContentManager(ContentManager contentManager)
    {
        mContentManager = contentManager;
    }

    public void LoadTexture(string id, string fileName)
    {
        Texture2D texture = mContentManager.Load<Texture2D>(fileName);
        mTextures.Add(id, texture);
    }

    public void LoadSpriteTexture(string id, string fileName)
    {
        SpriteFont spriteFont = mContentManager.Load<SpriteFont>(fileName);
        mSpriteFonts.Add(id, spriteFont);
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

    // render textures
    public void DrawGameObject(GameObject.GameObject obj)
    {
        mSpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, obj.TextureColor, obj.Rotation, obj.TextureOffset,
            obj.TextureSclae, SpriteEffects.None, obj.TextureDepth/1000);
    }
    public void DrawGameObject(GameObject.GameObject obj, bool isHover)
    {                                            
        var color = isHover ? Globals.HoverColor : obj.TextureColor;
        mSpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, color, obj.Rotation, obj.TextureOffset,
            obj.TextureSclae, SpriteEffects.None, obj.TextureDepth / 1000);
    }

    public void Draw(string id, Vector2 position, int width, int height)
    {
        mSpriteBatch.Draw(GetTexture(id), new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
    }
    public void Draw(string id, Vector2 position, Vector2 offset, float sclae, float rotation, float depth, Color color)
    {
        mSpriteBatch.Draw(GetTexture(id), position, null, color, rotation, offset, sclae, SpriteEffects.None, depth/1000);
    }

    // render String
    public void DrawString(string id, Vector2 position, string text, Color color)
    {
        mSpriteBatch.DrawString(GetSpriteFont(id), text, position, color);
    }

    // render Circle
    public void DrawCircle(Vector2 center, float radius, Color color, float thickness, int depth)
    {
        mSpriteBatch.DrawCircle(center, radius, 90, color, thickness / Globals.Camera2d.mZoom, depth/1000);
    }

    // render Line
    public void DrawAdaptiveLine(Vector2 start, Vector2 end, Color color, float thickness, float depth)
    {
        mSpriteBatch.DrawLine(start, end, color, thickness / Globals.Camera2d.mZoom, depth / 1000);
    }

    public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness, float depth)
    {
        mSpriteBatch.DrawLine(start, end, color, thickness, depth/1000);
    }

}