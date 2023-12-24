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
    [JsonIgnore] public readonly bool UpdateBelow ;
    [JsonIgnore] public LayerManager LayerManager { get; private set; } 
    [JsonIgnore] protected Game1 mGame1;
    [JsonIgnore] protected GraphicsDevice mGraphicsDevice;
    [JsonIgnore] protected PersistanceManager mPersistanceManager;

    protected Layer(bool updateBelow) { UpdateBelow = updateBelow; }

    public virtual void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager)
    {
        mGame1 = game1;
        LayerManager = layerManager;
        mGraphicsDevice = graphicsDevice;
        mPersistanceManager = persistanceManager;
    }

    public abstract void Update(GameTime gameTime, InputState inputState);

    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Destroy();

    public abstract void OnResolutionChanged();

}