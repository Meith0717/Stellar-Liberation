// Layer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using System;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement;

[Serializable]
public abstract class Layer
{
    [JsonIgnore] public readonly bool UpdateBelow;
    [JsonIgnore] public LayerManager LayerManager { get; private set; }
    [JsonIgnore] protected Game1 Game1 { get; private set; }
    [JsonIgnore] protected GraphicsDevice GraphicsDevice { get; private set; }
    [JsonIgnore] protected PersistanceManager PersistanceManager { get; private set; }
    [JsonIgnore] public GameSettings GameSettings { get; private set; }
    [JsonIgnore] protected ResolutionManager ResolutionManager { get; private set; }

    protected Layer(bool updateBelow) { UpdateBelow = updateBelow; }

    public virtual void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager, GameSettings gameSettings, ResolutionManager resolutionManager)
    {
        Game1 = game1;
        LayerManager = layerManager;
        GraphicsDevice = graphicsDevice;
        PersistanceManager = persistanceManager;
        GameSettings = gameSettings;
        ResolutionManager = resolutionManager;
    }

    public abstract void Update(GameTime gameTime, InputState inputState);

    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Destroy();

    public abstract void OnResolutionChanged();

}