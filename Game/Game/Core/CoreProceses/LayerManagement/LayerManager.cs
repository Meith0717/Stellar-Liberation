// LayerManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement;

public class LayerManager
{
    private readonly Game1 mGame1;

    // layer stack
    private readonly LinkedList<Layer> mLayerStack = new();

    public LayerManager(Game1 game1)
    {
        mGame1 = game1;
    }

    // add and remove layers from stack
    public void AddLayer(Layer layer)
    {
        layer.Initialize();
        layer.ApplyResolution();
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
    public void OnResolutionChanged()
    {
        foreach (Layer layer in mLayerStack.ToArray()) layer.ApplyResolution();
    }

    public bool ContainsLayer(Layer layer) => mLayerStack.Contains(layer);
}