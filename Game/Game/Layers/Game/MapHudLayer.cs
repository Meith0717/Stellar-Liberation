﻿// MapHudLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Layers.MenueLayers;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class MapHudLayer : Layer
    {
        private readonly UiCanvas mMainFrame;
        private readonly MapLayer mMapLayer;

        public MapHudLayer(GameState gameState, MapLayer mapLayer, Game1 game1) : base(game1, true)
        {
            mMapLayer = mapLayer;
            mMainFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            UiGrid grid;
            mMainFrame.AddChild(grid = new(5, 1) { Anchor = Anchor.SE, Width = 300, Height = 40 });
            grid.Set(3, 0, new UiButton("planetSystemButton", "") { FillScale = FillScale.Fit, Anchor = Anchor.S, OnClickAction = gameState.CloseMap });
            grid.Set(4, 0, new UiButton("pauseButton", "") { FillScale = FillScale.Fit, Anchor = Anchor.S, OnClickAction = () => LayerManager.AddLayer(new PauseLayer(gameState, Game1)) });
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            mMainFrame.Update(inputState, gameTime);
        }

        public override void ApplyResolution()
        {
            base.ApplyResolution();
            mMainFrame.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mMainFrame.Draw();
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: mMapLayer.ViewTransformationMatrix);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}

