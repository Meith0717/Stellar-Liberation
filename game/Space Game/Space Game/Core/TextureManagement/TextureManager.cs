using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace rache_der_reti.Core.TextureManagement;

public class TextureManager
{
    private static TextureManager sInstance;

    private SpriteBatch mSpriteBatch;
    private ContentManager mContentManager;

    readonly Hashtable mTextures = new();
    readonly Hashtable mSpriteFonts = new();

    private const int MaxLayerHeight = 10000;


    public static TextureManager GetInstance()
    {
        return sInstance ??= new TextureManager();
    }

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
        Texture2D texture =  (Texture2D)mTextures[id];
        if (texture == null)
        {
            throw new System.Exception("Error, Texture was not found!");
        }

        return texture;
    }
    public SpriteFont GetSpriteFont(string id)
    {
        SpriteFont spriteFont =  (SpriteFont)mSpriteFonts[id];
        if (spriteFont == null)
        {
            throw new System.Exception("Error, Texture was not found!");
        }

        return spriteFont;
    }

    // render textures
    
    // ReSharper disable once UnusedMember.Global
    public void Draw(string id, Vector2 position)
    {
        mSpriteBatch.Draw(GetTexture(id), position, Color.White);
    }
    
    public void Draw(string id, Vector2 position, int width, int height)
    {
        mSpriteBatch.Draw(GetTexture(id), new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);
    }
    
    // ReSharper disable once UnusedMember.Global
    public void Draw(string id, Vector2 position, int width, int height, float objectY, bool flip = false)
    {
        // add flip effect
        SpriteEffects effects = SpriteEffects.None;
        if (flip)
        {
            effects = SpriteEffects.FlipHorizontally;
        }
        mSpriteBatch.Draw(GetTexture(id), new Rectangle((int)position.X, (int)position.Y, width, height), 
            null, Color.White, 0, Vector2.Zero, effects, objectY / MaxLayerHeight);
    }
    
    public void DrawFrame(string id, Vector2 position, int width, int height, int frameCount, int totalFrames, bool flip, float objectY = 0)
    {
        // check bounds of frame
        if (frameCount >= totalFrames || frameCount < 0)
        {
            throw new System.Exception("Error, frame count not in range");
        }

        // add flip effect
        SpriteEffects effects = SpriteEffects.None;
        if (flip)
        {
            effects = SpriteEffects.FlipHorizontally;
        }
        
        // get texture and draw
        Texture2D texture = GetTexture(id);
        int frameWidth = texture.Width / totalFrames;
        mSpriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), 
            new Rectangle(frameWidth * frameCount, 0, frameWidth, texture.Height),
            Color.White, 0, Vector2.Zero, effects, objectY / MaxLayerHeight);
    }
    
    // render text
    public void DrawString(string id, Vector2 position, string text, Color color)
    {
        mSpriteBatch.DrawString(GetSpriteFont(id), text, position, color);
    }

    public void DrawRectangle(string id, Rectangle rectangle)
    {
        Texture2D texture = GetTexture(id);
        mSpriteBatch.Draw(texture, rectangle, Color.White);

        /*// create a slightly smaller inner rectangle.
        Rectangle innerRectangle = rectangle;
        innerRectangle.Inflate(-1, -1);
        mSpriteBatch.Draw(texture, innerRectangle, Color.Transparent);*/
    }
}