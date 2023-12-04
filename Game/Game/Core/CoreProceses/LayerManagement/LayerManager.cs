// LayerManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement;

public class LayerManager
{
    private readonly Game1 mGame1;
    private readonly GraphicsDevice mGraphicsDevice;
    private readonly Serialize mSerialize;

    // layer stack
    private readonly LinkedList<Layer> mLayerStack = new LinkedList<Layer>();

    public LayerManager(Game1 game1, GraphicsDevice graphicsDevice, Serialize serialize)
    {
        mGame1 = game1;
        mGraphicsDevice = graphicsDevice;
        mSerialize = serialize;
    }

    // add and remove layers from stack
    public void AddLayer(Layer layer)
    {
        mLayerStack.AddLast(layer);
        layer.Initialize(mGame1, this, mGraphicsDevice, mSerialize);
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
        foreach (Layer layer in mLayerStack.Reverse())
        {
            layer.Update(gameTime, inputState);
            if (!layer.UpdateBelow) break;
        }
    }

    // draw layers
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Layer layer in mLayerStack)
        {
            layer.Draw(spriteBatch);
        }
    }

    // lifecycle methods
    public void Exit()
    {
        foreach (Layer layer in mLayerStack)
        {
            layer.Destroy();
        }
        mGame1.Exit();
    }

    // fullscreen stuff
    public void OnResolutionChanged()
    {
        foreach (Layer layer in mLayerStack.ToArray())
        {
            layer.OnResolutionChanged();
        }
    }

    public bool ContainsLayer(Layer layer)
    {
        return mLayerStack.Contains(layer);
    }
}