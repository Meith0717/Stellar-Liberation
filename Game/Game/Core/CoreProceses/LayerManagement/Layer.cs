// Layer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using System;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement;

[Serializable]
public abstract class Layer
{
    protected Game1 mGame1;
    protected LayerManager mLayerManager;
    protected GraphicsDevice mGraphicsDevice;
    protected Serialize mSerialize;

    [JsonProperty]
    public bool UpdateBelow { get; private set; }

    protected Layer(bool updateBelow) { UpdateBelow = updateBelow; }

    public virtual void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
    {
        mGame1 = game1;
        mLayerManager = layerManager;
        mGraphicsDevice = graphicsDevice;
        mSerialize = serialize;
    }

    public abstract void Update(GameTime gameTime, InputState inputState);

    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Destroy();

    public abstract void OnResolutionChanged();

}