using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.SoundManagement;

namespace rache_der_reti.Core.LayerManagement;

[Serializable]
public abstract class Layer
{
    protected GraphicsDevice mGraphicsDevice;
    protected SpriteBatch mSpriteBatch;
    protected LayerManager mLayerManager;
    protected ContentManager mContentManager;

    // managers
    protected SoundManager mSoundManager;

    [JsonProperty]
    public bool UpdateBelow { get; set; }

    protected Layer()
    {}
    protected Layer(LayerManager layerManager, GraphicsDevice graphicsDevice,
        SpriteBatch spriteBatch, ContentManager contentManager, SoundManager soundManager)
    {
        mLayerManager = layerManager;
        mGraphicsDevice = graphicsDevice;
        mSpriteBatch = spriteBatch;
        mContentManager = contentManager;
        mSoundManager = soundManager;
    }

    public abstract void Update(GameTime gameTime, InputState inputState);

    public abstract void Draw();

    public abstract void Destroy();

    public abstract void OnResolutionChanged();
}