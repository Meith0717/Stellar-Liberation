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
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using StellarLiberation.Game.Layers.MenueLayers;
using System.Linq;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class PlanetSystemHudLayer : Layer
    {
        private readonly UiFrame mMainFrame;
        private readonly PlanetsystemLayer mPlanetsystemLayer;
        private readonly GameState mGameState;

        public PlanetSystemHudLayer(GameState gameState, PlanetsystemLayer planetsystemLayer, Game1 game1) : base(game1, true)
        {
            mGameState = gameState;
            mPlanetsystemLayer = planetsystemLayer;
            mMainFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };
            mMainFrame.AddChild(new UiButton("pauseButton", "") { Anchor = Anchor.NE, HSpace = 20, VSpace = 20, OnClickAction = () => LayerManager.AddLayer(new PauseLayer(gameState, Game1)) });
            mMainFrame.AddChild(new UiButton("mapButton", "") { Anchor = Anchor.SE, HSpace = 20, VSpace = 20, OnClickAction = gameState.OpenMap });
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

            spriteBatch.Begin(transformMatrix: mPlanetsystemLayer.ViewTransformationMatrix);
            mGameState.GameObjectsInteractor.Draw(mPlanetsystemLayer.Camera2D);
            foreach (var spaceship in mPlanetsystemLayer.PlanetsystemState.GameObjects.OfType<Spacecraft>())
                TextureManager.Instance.Draw("radar", spaceship.Position, .04f / mPlanetsystemLayer.Camera2D.Zoom, 0, spaceship.TextureDepth + 1, spaceship.Fraction == Fraction.Enemys ? Color.Red : Color.LightGreen);

            foreach (var spaceship in mGameState.GameObjectsInteractor.SelectedFlagships)
            {
                if (spaceship.Fraction != Fraction.Allied) continue;
                spaceship.NavigationSystem.DrawImpulseDriveWayPoints(spaceship, mPlanetsystemLayer.PlanetsystemState);

                if (!mPlanetsystemLayer.PlanetsystemState.Contains(spaceship)) continue;
                TextureManager.Instance.Draw("selectCrosshait", spaceship.Position, 2.5f, 0, 1, Color.MonoGameOrange);

                foreach (var hangarShips in spaceship.Hangar.InSpaceShips)
                    TextureManager.Instance.Draw("radar", hangarShips.Position, .04f / mPlanetsystemLayer.Camera2D.Zoom, 0, hangarShips.TextureDepth + 1, Color.MonoGameOrange);
            }

            foreach (var planet in mPlanetsystemLayer.PlanetsystemState.Planets)
                TextureManager.Instance.DrawAdaptiveCircle(Vector2.Zero, planet.Position.Length(), Color.Gray * .1f, 1, planet.TextureDepth - 1, mPlanetsystemLayer.Camera2D.Zoom);
            spriteBatch.End();

            base.Draw(spriteBatch);
        }
    }
}

