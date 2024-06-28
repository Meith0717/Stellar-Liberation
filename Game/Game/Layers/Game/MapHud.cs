// PlanetsystemHud.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using StellarLiberation.Game.Layers.MenueLayers;
using System.Linq;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class MapHud : Layer
    {
        private readonly UiFrame mMainFrame;
        private readonly GameState mGameState;

        public MapHud(Game1 game1, GameState gameState) : base(game1, true)
        {
            mGameState = gameState;
            mMainFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };

            mMainFrame.AddChild(new UiButton("pauseButton", "") { Anchor = Anchor.NE, HSpace = 20, VSpace = 20, OnClickAction = () => LayerManager.AddLayer(new PauseLayer(gameState, Game1)) });
            mMainFrame.AddChild(new UiButton("planetSystemButton", "") { Anchor = Anchor.SE, HSpace = 20, VSpace = 20, OnClickAction = gameState.PopLayer });
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            inputState.DoAction(ActionType.ToggleHyperMap, mGameState.PopLayer);
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

            base.Draw(spriteBatch);
        }
    }
}

