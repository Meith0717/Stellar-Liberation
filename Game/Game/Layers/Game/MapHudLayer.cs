// PlanetsystemHud.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.Layers.MenueLayers;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class MapHudLayer : Layer
    {
        private readonly UiFrame mMainFrame;
        private readonly MapLayer mMapLayer;

        public MapHudLayer(GameState gameState, MapLayer mapLayer, Game1 game1) : base(game1, true)
        {
            mMapLayer = mapLayer;
            mMainFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mMainFrame.AddChild(new UiButton("pauseButton", "") { Anchor = Anchor.NE, HSpace = 20, VSpace = 20, OnClickAction = () => LayerManager.AddLayer(new PauseLayer(gameState, Game1)) });
            mMainFrame.AddChild(new UiButton("planetSystemButton", "") { Anchor = Anchor.SE, HSpace = 20, VSpace = 20, OnClickAction = gameState.CloseMap });
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

