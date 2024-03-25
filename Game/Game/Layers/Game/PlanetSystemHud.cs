// HudLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GridSystem;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.GameObjects.AstronomicalObjects;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using StellarLiberation.Game.Layers.MenueLayers;
using System.Collections.Generic;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class PlanetSystemHud : Layer
    {
        public bool Hide;

        private readonly UiFrame mMainFrame;
        private readonly PlanetSystemLayer mPlanetSystemLayer;
        private readonly Grid mGrid;
        private readonly List<UiFrame> mPopups = new();
        private UiFrame mBottomLeftMenue = new();

        public UiFrame SetLeftMenue { set { mBottomLeftMenue = value; } }

        public PlanetSystemHud(PlanetSystemLayer planetSystemLayer, Game1 game1) : base(game1, true)
        {
            mMainFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };

            mMainFrame.AddChild(new UiButton(MenueSpriteRegistries.pause, "") { Anchor = Anchor.NE, HSpace = 20, VSpace = 20, OnClickAction = () => LayerManager.AddLayer(new PauseLayer(planetSystemLayer.GameState, Game1)) });
            mPlanetSystemLayer = planetSystemLayer;
            mGrid = new(10000);

        }

        public void AddPopup(UiFrame uiFrame)
        {
            uiFrame.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
            mPopups.Add(uiFrame);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.F1, () => Hide = !Hide);
            if (Hide) return;

            mMainFrame.Update(inputState, gameTime);
            foreach (var uiFranes in mPopups)
                uiFranes.Update(inputState, gameTime);
        }

        public override void ApplyResolution()
        {
            mMainFrame.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
            foreach (var uiFranes in mPopups)
                uiFranes.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide) return;

            spriteBatch.Begin(transformMatrix: mPlanetSystemLayer.ViewTransformationMatrix);
            mGrid.Draw(mPlanetSystemLayer);
            foreach (var spaceship in mPlanetSystemLayer.SpaceShipInteractor.SelectedSpaceships)
                TextureManager.Instance.DrawCircle(spaceship.Position, spaceship.BoundedBox.Radius, Color.Purple, 10, 1);
            if (mPlanetSystemLayer.SpaceShipInteractor.HoveredGameObject is not null)
            {
                var obi = mPlanetSystemLayer.SpaceShipInteractor.HoveredGameObject;
                TextureManager.Instance.DrawAdaptiveCircle(obi.Position, obi.BoundedBox.Radius, Color.White * .4f, 2, 1, mPlanetSystemLayer.Camera2D.Zoom);
            }
            foreach (var spaceship in mPlanetSystemLayer.GameObjectsManager.OfType<Spaceship>())
                TextureManager.Instance.Draw(GameSpriteRegistries.radar, spaceship.Position, .04f / mPlanetSystemLayer.Camera2D.Zoom, 0, spaceship.TextureDepth + 1, spaceship.Fraction == Fractions.Enemys ? Color.Red : Color.LightGreen);
            foreach (var planet in mPlanetSystemLayer.GameObjectsManager.OfType<Planet>())
                TextureManager.Instance.DrawAdaptiveCircle(Vector2.Zero, planet.OrbitRadius, Color.Gray * .1f, 1, planet.TextureDepth - 1, mPlanetSystemLayer.Camera2D.Zoom);
            spriteBatch.End();

            spriteBatch.Begin();
            mMainFrame.Draw();
            foreach (var uiFrame in mPopups)
                uiFrame.Draw();
            spriteBatch.End();
        }
    }
}
