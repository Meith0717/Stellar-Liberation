// Layer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.CoreProceses.LayerManagement;

[Serializable]
public abstract class Layer
{
    [JsonIgnore] public readonly bool UpdateBelow;
    [JsonIgnore] public readonly bool DrawBelow;
    [JsonIgnore] public bool Blur;
    [JsonIgnore] public readonly LayerManager LayerManager;
    [JsonIgnore] public readonly GameSettings GameSettings;
    [JsonIgnore] protected readonly ResolutionManager ResolutionManager;
    [JsonIgnore] protected readonly Game1 Game1;
    [JsonIgnore] protected readonly GraphicsDevice GraphicsDevice;
    [JsonIgnore] protected readonly PersistanceManager PersistanceManager;
    [JsonIgnore] private readonly List<UiElement> mUiElements;
    [JsonIgnore] private readonly UiText mHoverText;
    [JsonIgnore] private RenderTarget2D mRenderTarget2D;

    protected Layer(Game1 game1, bool updateBelow, bool drawBelow = true)
    {
        Game1 = game1;
        LayerManager = game1.LayerManager;
        GraphicsDevice = game1.GraphicsDevice;
        PersistanceManager = game1.PersistanceManager;
        GameSettings = game1.Settings;
        ResolutionManager = game1.ResolutionManager;
        UpdateBelow = updateBelow;
        mUiElements = new();
        mHoverText = new UiText("neuropolitical", "", .1f);
        DrawBelow = drawBelow;
        mRenderTarget2D = new(game1.GraphicsDevice, game1.GraphicsManager.PreferredBackBufferWidth, game1.GraphicsManager.PreferredBackBufferHeight);
    }

    public virtual void Initialize() {; }

    public void AddUiElement(UiElement uiElement)
    {
        uiElement.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        mUiElements.Add(uiElement);
    }

    public void SetHoverText(string text) => mHoverText.Text = text;

    public void ClearUiElements() => mUiElements.Clear();

    public virtual void Update(GameTime gameTime, InputState inputState)
    {
        mHoverText.X = (int)inputState.mMousePosition.X;
        mHoverText.Y = (int)inputState.mMousePosition.Y - 15;
        mHoverText.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        foreach (UiElement uiElement in mUiElements.ToList())
        {
            uiElement.Update(inputState, gameTime);
            if (!uiElement.IsDisposed) continue;
            mUiElements.Remove(uiElement);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        foreach (UiElement uiElement in mUiElements) uiElement.Draw();
        mHoverText.Draw();
        spriteBatch.End();
        mHoverText.Text = "";
    }

    public RenderTarget2D GetRenderTarget(SpriteBatch spriteBatch)
    {
        GraphicsDevice.SetRenderTarget(mRenderTarget2D);
        GraphicsDevice.Clear(Color.Transparent);
        Draw(spriteBatch);
        GraphicsDevice.SetRenderTarget(null);
        return mRenderTarget2D;
    }

    public virtual void Destroy()
    {
        ClearUiElements();
    }

    public virtual void ApplyResolution()
    {
        mRenderTarget2D.Dispose();
        foreach (UiElement uiElement in mUiElements)
            uiElement.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        mHoverText.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        mRenderTarget2D = new(Game1.GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
    }
}