using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.SoundManagement;
using System;
using Galaxy_Explovive.Core.TextureManagement;

namespace Galaxy_Explovive.Core.LayerManagement;

[Serializable]
public abstract class Layer
{
    protected LayerManager mLayerManager;
    public SoundManager mSoundManager;
    public TextureManager mTextureManager;

    [JsonProperty]
    public bool UpdateBelow { get; set; }

    public Layer(LayerManager layerManager, SoundManager soundManager, TextureManager textureManager)
    {
        mLayerManager = layerManager;
        mSoundManager = soundManager;
        mTextureManager = textureManager;
    }

    public abstract void Update(GameTime gameTime, InputState inputState);

    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Destroy();

    public abstract void OnResolutionChanged();
}