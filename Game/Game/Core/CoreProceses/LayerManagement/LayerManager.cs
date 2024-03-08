// LayerManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement;

public class LayerManager
{
    private readonly Game1 mGame1;
    private readonly GraphicsDevice mGraphicsDevice;
    private readonly PersistanceManager mPersistanceManager;
    private readonly ResolutionManager mResolutionManager;
    private readonly GameSettings mGameSettings;

    // layer stack
    private readonly LinkedList<Layer> mLayerStack = new();

    public LayerManager(Game1 game1, GraphicsDevice graphicsDevice, PersistanceManager persistanceManager, ResolutionManager resolutionManager, GameSettings gameSettings)
    {
        mGame1 = game1;
        mGraphicsDevice = graphicsDevice;
        mResolutionManager = resolutionManager;
        mPersistanceManager = persistanceManager;
        mGameSettings = gameSettings;
    }

    // add and remove layers from stack
    public void AddLayer(Layer layer)
    {
        layer.Initialize(mGame1, this, mGraphicsDevice, mPersistanceManager, mGameSettings, mResolutionManager);
        mLayerStack.AddLast(layer);
    }

    public void PopLayer()
    {
        if (mLayerStack.Last != null)
        {
            mLayerStack.Last.Value.Destroy();
            mLayerStack.RemoveLast();
        }
    }

    // update layers
    public void Update(GameTime gameTime, InputState inputState)
    {
        if (mResolutionManager.WasResized) OnResolutionChanged();

        var reversedStack = mLayerStack.Reverse();
        foreach (Layer layer in reversedStack)
        {
            layer.Update(gameTime, inputState);
            if (!layer.UpdateBelow) break;
        }
    }

    // draw layers
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Layer layer in mLayerStack.ToList())
        {
            layer.Draw(spriteBatch);
        }
    }

    // lifecycle methods
    public void Exit()
    {
        foreach (Layer layer in mLayerStack) layer.Destroy();
        mGame1.Exit();
    }

    // fullscreen stuff
    private void OnResolutionChanged()
    {
        foreach (Layer layer in mLayerStack.ToArray()) layer.OnResolutionChanged();
    }

    public bool ContainsLayer(Layer layer) => mLayerStack.Contains(layer);
}