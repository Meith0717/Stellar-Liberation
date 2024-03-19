// HudLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.Layers.MenueLayers;
using System.Collections.Generic;

namespace StellarLiberation.Game.Layers.GameLayers
{
    public class HudLayer : Layer
    {
        public bool Hide;

        private readonly UiFrame mUiLayer;
        private readonly GameLayer mScene;

        private readonly List<UiElement> mPopups = new();

        public HudLayer(GameLayer gameLayer) : base(true)
        {
            mUiLayer = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };

            mUiLayer.AddChild(new UiButton(MenueSpriteRegistries.pause, "") { Anchor = Anchor.NE, HSpace = 20, VSpace = 20, OnClickAction = () => LayerManager.AddLayer(new PauseLayer(gameLayer.GameState)) });
            mScene = gameLayer;
        }

        public void AddPopup(UiElement uiElement) => mPopups.Add(uiElement);

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide) return;
            spriteBatch.Begin();
            mUiLayer.Draw();
            foreach (var uiElement in mPopups)
                uiElement.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged() { }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mUiLayer.Update(inputState, gameTime, GraphicsDevice.Viewport.Bounds, ResolutionManager.UiScaling);
            foreach (var uiElement in mPopups)
                uiElement.Update(inputState, gameTime, GraphicsDevice.Viewport.Bounds, ResolutionManager.UiScaling);
            mPopups.Clear();
        }
    }
}
