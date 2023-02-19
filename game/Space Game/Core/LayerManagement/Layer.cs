using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.SoundManagement;
using Space_Game.Core;

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
    {
        mLayerManager = Globals.mLayerManager;
        mGraphicsDevice = Globals.mGraphicsDevice;
        mSpriteBatch = Globals.mSpriteBatch;
        mContentManager = Globals.mContentManager;
        mSoundManager = Globals.mSoundManager;
    }

    public abstract void Update(GameTime gameTime, InputState inputState);

    public abstract void Draw();

    public abstract void Destroy();

    public abstract void OnResolutionChanged();
}