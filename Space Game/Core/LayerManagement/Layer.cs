using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.SoundManagement;
using System;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.Persistance;

namespace Galaxy_Explovive.Core.LayerManagement;

[Serializable]
public abstract class Layer
{
    protected Game1 mApp;
    protected LayerManager mLayerManager;
    protected GraphicsDevice mGraphicsDevice;
    public SoundManager mSoundManager;
    public TextureManager mTextureManager;
    public Serialize mSerialize;

    [JsonProperty]
    public bool UpdateBelow { get; set; }

    public Layer(Game1 app)
    {
        mApp = app;
        mLayerManager = app.mLayerManager;
        mSoundManager = app.mSoundManager;
        mTextureManager = app.mTextureManager;
        mGraphicsDevice = app.GraphicsDevice;
        mSerialize = app.mSerialize;
    }

    public abstract void Update(GameTime gameTime, InputState inputState);

    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Destroy();

    public abstract void OnResolutionChanged();
}