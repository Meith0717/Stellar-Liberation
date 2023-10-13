// LayerManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.LayerManagement;

public class LayerManager
{
    private readonly Game1 mGame1;
    private readonly GraphicsDevice mGraphicsDevice;
    private readonly Persistance.Serialize mSerialize;

    // layer stack
    private readonly LinkedList<Layer> mLayerStack = new LinkedList<Layer>();

    public LayerManager(Game1 game1, GraphicsDevice graphicsDevice, Persistance.Serialize serialize)
    {
        mGame1 = game1;
        mGraphicsDevice = graphicsDevice;
        mSerialize = serialize;
    }

    // add and remove layers from stack
    public void AddLayer(Layer layer)
    {
        layer.Initialize(mGame1, this, mGraphicsDevice, mSerialize);
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