using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rache_der_reti.Core.TextureManagement;

namespace rache_der_reti.Core.Menu;

public class UiElementSprite : UiElement
{
    internal string SpriteId { get; set; }

    public SpriteFit mSpriteFit = SpriteFit.Fit;

    private bool mHoverEnabled;
    public int mActiveFrame = 0;
    private bool mToggleEnabled;
    public bool mToggle = true;
    private int mTotalFrames = 1;

    public enum SpriteFit
    {
        Fit,  // sprite is fit into frame without distortion
        Cover,  // sprite gets distorted to the frame
        Fixed,  // sprite is original size
        Fill  // sprite is filling the frame without distortion
    }

    public void EnableHover(int totalFrames)
    {
        mHoverEnabled = true;
        mTotalFrames = totalFrames;
    }

    public void EnableMultipleFrames(int totalFrames)
    {
        mTotalFrames = totalFrames;
    }

    public void EnableToggle(bool toggle)
    {
        mTotalFrames = 2;
        mToggleEnabled = true;
        mToggle = toggle;
    }

    // constructors
    public UiElementSprite(string spriteId)
    {
        SpriteId = spriteId;
    }

    public override void Update(Rectangle rectangle)
    {
        base.Update(rectangle);
        Vector2 offset = Vector2.Zero;

        Texture2D texture = TextureManager.GetInstance().GetTexture(SpriteId);
        
        float textureAspectRatio = (float)texture.Width / texture.Height / mTotalFrames;
        float availableAspectRatio = (float)CalculatedRectangle.Width / CalculatedRectangle.Height;

        int width = CalculatedRectangle.Width;
        int height = CalculatedRectangle.Height;
        switch(mSpriteFit)
        {
            case SpriteFit.Fit:
                if (textureAspectRatio < availableAspectRatio)
                {
                    width = (int)(CalculatedRectangle.Height * textureAspectRatio);
                    offset = new Vector2((CalculatedRectangle.Width - width) / 2f, 0);
                }
                else
                {
                    height = (int)(CalculatedRectangle.Width / textureAspectRatio);
                    offset = new Vector2(0, (CalculatedRectangle.Height - height) / 2f);
                }
                break;
            case SpriteFit.Fixed:
                width = texture.Width;
                height = texture.Height;
                break;
            case SpriteFit.Fill:
                if (textureAspectRatio < availableAspectRatio)
                {
                    height = (int)(CalculatedRectangle.Width / textureAspectRatio);
                    offset = new Vector2(0, (CalculatedRectangle.Height - height) / 2f);
                }
                else
                {
                    width = (int)(CalculatedRectangle.Height * textureAspectRatio);
                    offset = new Vector2((CalculatedRectangle.Width - width) / 2f, 0);
                }
                break;
            case SpriteFit.Cover:
                break;
        }
        CalculatedRectangle = new Rectangle((int)(CalculatedRectangle.X + offset.X), (int)(CalculatedRectangle.Y + offset.Y), width, height);
    }

    public override void Render()
    {
        base.Render();
        int frameCount = mActiveFrame;
        if (mHoverEnabled && mIsHovering)
        {
            frameCount = 1; 
        }
        else if (mToggleEnabled && !mToggle)
        {
            frameCount = 1;
        }
        TextureManager.GetInstance().DrawFrame(SpriteId, new Vector2(CalculatedRectangle.X, CalculatedRectangle.Y), 
            CalculatedRectangle.Width, CalculatedRectangle.Height, frameCount, mTotalFrames, false);

        base.Render();
    }
    
    protected override Rectangle GetClickBox()
    {
        return CalculatedRectangle;
    }
    
    protected override void OnClick()
    {
        mToggle = !mToggle;
    }
}