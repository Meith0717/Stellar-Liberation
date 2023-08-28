using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.LayerManagement;

[Serializable]
public abstract class Layer
{
    public LayerManager mLayerManager { get; private set; }
    public GraphicsDevice mGraphicsDevice { get; private set; }

    [JsonProperty]
    public bool UpdateBelow { get; set; }

    public Layer(bool updateBelow)
    {
        UpdateBelow = updateBelow;
    }

    public void Initialize(LayerManager layerManager, GraphicsDevice graphicsDevice)
    {
        mLayerManager = layerManager;
        mGraphicsDevice = graphicsDevice;
    }

    public abstract void Update(GameTime gameTime, InputState inputState);

    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Destroy();

    public abstract void OnResolutionChanged();
}