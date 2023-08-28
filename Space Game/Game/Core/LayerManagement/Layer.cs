using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.Persistance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.Core.LayerManagement;

[Serializable]
public abstract class Layer
{
    [JsonIgnore] public LayerManager LayerManager { get; private set; }
    [JsonIgnore] public GraphicsDevice GraphicsDevice { get; private set; }
    [JsonIgnore] public Serialize Serialize { get; private set; }

    [JsonProperty]
    public bool UpdateBelow { get; set; }

    public Layer(bool updateBelow)
    {
        UpdateBelow = updateBelow;
    }

    public void Initialize(LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
    {
        LayerManager = layerManager;
        GraphicsDevice = graphicsDevice;
        Serialize = serialize;
    }

    public abstract void Update(GameTime gameTime, InputState inputState);

    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Destroy();

    public abstract void OnResolutionChanged();
}