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
    const int MaxLayerHeight = 10000;

    private readonly Hashtable mTextures = new();
    private readonly Hashtable mSpriteFonts = new();
    private readonly ContentManager mContentManager;
    public  SpriteBatch SpriteBatch { get; private set; }
    private float mCamZoom; 

    public TextureManager(ContentManager contentManager) 
    {
        mContentManager = contentManager;
    }

    public void SetCamZoom(float camZoom)
    {
        mCamZoom = camZoom;
    }

    public void SetSpriteBatch(SpriteBatch spriteBatch)
    {
        SpriteBatch = spriteBatch;
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

    // render Textures ___________________________________________________________________________

    public void Draw(string id, Vector2 position, int width, int height)
    {
        SpriteBatch.Draw(GetTexture(id), new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
    }
    public void Draw(string id, Vector2 position, Vector2 offset, float sclae, float rotation, float depth, Color color)
    {
        SpriteBatch.Draw(GetTexture(id), position, null, color, rotation, offset, sclae, SpriteEffects.None, depth / MaxLayerHeight);
    }

    // render Game Objects ___________________________________________________________________________
    public void DrawGameObject(GameObject.GameObject obj)
    {
        SpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, obj.TextureColor, obj.Rotation, obj.TextureOffset,
            obj.TextureScale, SpriteEffects.None, obj.TextureDepth / MaxLayerHeight);
    }
    public void DrawGameObject(GameObject.GameObject obj, bool isHover)
    {                                            
        var color = isHover ? Globals.HoverColor : obj.TextureColor;
        SpriteBatch.Draw(GetTexture(obj.TextureId), obj.Position, null, color, obj.Rotation, obj.TextureOffset,
            obj.TextureScale, SpriteEffects.None, obj.TextureDepth / MaxLayerHeight);
    }

    // render String
    public void DrawString(string id, Vector2 position, string text, Color color)
    {
        SpriteBatch.DrawString(GetSpriteFont(id), text, position, color);
    }

    // render Circle ___________________________________________________________________________
    public void DrawCircle(Vector2 center, float radius, Color color, float thickness, int depth)
    {
        SpriteBatch.DrawCircle(center, radius, 90, color, thickness, depth / MaxLayerHeight);
    }

    public void DrawAdaptiveCircle(Vector2 center, float radius, Color color, float thickness, int depth)
    {
        SpriteBatch.DrawCircle(center, radius, 90, color, thickness / mCamZoom, depth / MaxLayerHeight);
    }


    // render Line ___________________________________________________________________________
    public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness, float depth)
    {
        SpriteBatch.DrawLine(start, end, color, thickness, depth / MaxLayerHeight);
    }

    public void DrawAdaptiveLine(Vector2 start, Vector2 end, Color color, float thickness, float depth)
    {
        SpriteBatch.DrawLine(start, end, color, thickness / mCamZoom, depth / MaxLayerHeight);
    }


}