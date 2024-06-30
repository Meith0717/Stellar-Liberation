// LayerManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement;

public class LayerManager
{
    private readonly Game1 mGame1;

    // layer stack
    private readonly LinkedList<Layer> mLayerStack = new();
    private readonly LinkedList<RenderTarget2D> mRenderTarget2Ds = new();
    private readonly Effect mBlurEffect;
    private RenderTarget2D mBlurRenderTarget;

    public LayerManager(Game1 game1)
    {
        mGame1 = game1;
        mBlurEffect = EffectManager.Instance.Effect;
        mBlurEffect.Parameters["texelSize"].SetValue(new Vector2(1.0f / mGame1.GraphicsDevice.Viewport.Width, 1.0f / mGame1.GraphicsDevice.Viewport.Height));
        mBlurRenderTarget = new(game1.GraphicsDevice, game1.GraphicsManager.PreferredBackBufferWidth, game1.GraphicsManager.PreferredBackBufferHeight);
    }

    // add and remove layers from stack
    public void AddLayer(Layer layer)
    {
        mLayerStack.AddLast(layer);
        layer.Initialize();
        layer.ApplyResolution();
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
        var reversedStack = mLayerStack.Reverse();
        foreach (Layer layer in reversedStack)
        {
            mRenderTarget2Ds.AddLast(layer.GetRenderTarget(spriteBatch));
            if (!layer.DrawBelow) break;
        } 

        var firstRenderTarget = mRenderTarget2Ds.First();
        mRenderTarget2Ds.RemoveFirst();

        foreach (var rendertarget in mRenderTarget2Ds)
        {
            mGame1.GraphicsDevice.SetRenderTarget(mBlurRenderTarget);
            mGame1.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(effect: mBlurEffect);
            if (rendertarget == firstRenderTarget) continue;
            spriteBatch.Draw(mRenderTarget2Ds.First(), mGame1.GraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();
            mGame1.GraphicsDevice.SetRenderTarget(null);
        }

        spriteBatch.Begin(effect: mBlurEffect);
        spriteBatch.Draw(mBlurRenderTarget, mGame1.GraphicsDevice.Viewport.Bounds, Color.White);
        spriteBatch.End();

        spriteBatch.Begin();
        spriteBatch.Draw(firstRenderTarget, mGame1.GraphicsDevice.Viewport.Bounds, Color.White);
        spriteBatch.End();

        mRenderTarget2Ds.Clear();
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
        mBlurRenderTarget.Dispose();
        foreach (Layer layer in mLayerStack) layer.ApplyResolution();
        mBlurRenderTarget = new(mGame1.GraphicsDevice, mGame1.GraphicsManager.PreferredBackBufferWidth, mGame1.GraphicsManager.PreferredBackBufferHeight);
    }

    public bool ContainsLayer(Layer layer) => mLayerStack.Contains(layer);
}