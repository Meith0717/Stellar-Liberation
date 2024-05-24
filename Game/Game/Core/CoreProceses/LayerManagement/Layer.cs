// Layer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
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
    [JsonIgnore] public readonly LayerManager LayerManager;
    [JsonIgnore] public readonly GameSettings GameSettings;
    [JsonIgnore] protected readonly ResolutionManager ResolutionManager;
    [JsonIgnore] protected readonly Game1 Game1;
    [JsonIgnore] protected readonly GraphicsDevice GraphicsDevice;
    [JsonIgnore] protected readonly PersistanceManager PersistanceManager;
    [JsonIgnore] private readonly List<UiElement> mUiElements;
    [JsonIgnore] private readonly UiText mHoverText;

    protected Layer(Game1 game1, bool updateBelow)
    {
        Game1 = game1;
        LayerManager = game1.LayerManager;
        GraphicsDevice = game1.GraphicsDevice;
        PersistanceManager = game1.PersistanceManager;
        GameSettings = game1.Settings;
        ResolutionManager = game1.ResolutionManager;
        UpdateBelow = updateBelow;
        mUiElements = new();
        mHoverText = new UiText(FontRegistries.text, "", .1f);
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

    public virtual void Destroy()
    {
        ClearUiElements();
    }

    public virtual void ApplyResolution()
    {
        foreach (UiElement uiElement in mUiElements)
            uiElement.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        mHoverText.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
    }
}