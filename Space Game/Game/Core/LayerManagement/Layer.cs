using CelestialOdyssey.Core.GameEngine;
using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Persistance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.LayerManagement;

[Serializable]
public abstract class Layer
{
    protected Game1 mApp;
    protected LayerManager mLayerManager;
    protected GraphicsDevice mGraphicsDevice;
    public SoundManager mSoundManager;
    public Serialize mSerialize;

    [JsonProperty]
    public bool UpdateBelow { get; set; }

    public Layer(Game1 app)
    {
        mApp = app;
        mLayerManager = app.mLayerManager;
        mSoundManager = app.mSoundManager;
        mGraphicsDevice = app.GraphicsDevice;
        mSerialize = app.mSerialize;
    }

    public abstract void Update(GameTime gameTime, InputState inputState);

    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Destroy();

    public abstract void OnResolutionChanged();
}