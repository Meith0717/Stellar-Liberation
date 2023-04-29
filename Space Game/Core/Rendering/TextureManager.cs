using Galaxy_Explovive.Core.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections;

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
            throw new System.Exception("Error, Texture was not found!");
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
    public void Draw(string id, Vector2 position, Vector2 offset, int width, int height, float sclae, float rotation, float depth)
    {
        mSpriteBatch.Draw(GetTexture(id), position, null, Color.White, rotation, offset, sclae, SpriteEffects.None, depth);
    }
    public void Draw(string id, Vector2 position, Vector2 offset, int width, int height, float sclae, float rotation, float depth, Color color)
    {
        mSpriteBatch.Draw(GetTexture(id), position, null, color, rotation, offset, sclae, SpriteEffects.None, depth);
    }

    public void DrawString(string id, Vector2 position, string text, Color color)
    {
        mSpriteBatch.DrawString(GetSpriteFont(id), text, position, color);
    }

    public void DrawCircle(Vector2 center, float radius, Color color, float thickness, int depth)
    {
        mSpriteBatch.DrawCircle(center, radius, 90, color, thickness / Globals.mCamera2d.mZoom, depth);
    }

    public void DrawLine(Vector2 start, Vector2 end, Color color, float thickness, int depth)
    {
        mSpriteBatch.DrawLine(start, end, color, thickness / Globals.mCamera2d.mZoom, depth);
    }
}