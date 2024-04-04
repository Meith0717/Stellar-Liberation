﻿// PlanetsystemHud.cs 
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
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.UserInterface.UiElements;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using StellarLiberation.Game.Layers.MenueLayers;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class PlanetsystemHud : Layer
    {
        public bool Hide;

        private readonly UiFrame mMainFrame;
        private readonly PlanetsystemLayer mPlanetSystemLayer;

        public PlanetsystemHud(PlanetsystemLayer planetSystemLayer, Game1 game1) : base(game1, true)
        {
            mMainFrame = new() { RelWidth = 1, RelHeight = 1, Alpha = 0 };

            mMainFrame.AddChild(new UiButton(MenueSpriteRegistries.pause, "") { Anchor = Anchor.NE, HSpace = 20, VSpace = 20, OnClickAction = () => LayerManager.AddLayer(new PauseLayer(planetSystemLayer.GameState, Game1)) });
            mMainFrame.AddChild(new UiButton(MenueSpriteRegistries.map, "") { Anchor = Anchor.SE, HSpace = 20, VSpace = 20, OnClickAction = planetSystemLayer.OpenMap });
            mPlanetSystemLayer = planetSystemLayer;
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.F1, () => Hide = !Hide);
            if (Hide) return;
            base.Update(gameTime, inputState);
            inputState.DoAction(ActionType.ToggleHyperMap, mPlanetSystemLayer.OpenMap);


            mMainFrame.Update(inputState, gameTime);
        }

        public override void ApplyResolution()
        {
            base.ApplyResolution();
            mMainFrame.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide) return;

            spriteBatch.Begin(transformMatrix: mPlanetSystemLayer.ViewTransformationMatrix);
            foreach (var spaceship in mPlanetSystemLayer.GameState.GameObjectsInteractor.SelectedSpaceships)
                TextureManager.Instance.DrawCircle(spaceship.Position, spaceship.BoundedBox.Radius, Color.Purple, 10, 1);
            if (mPlanetSystemLayer.GameState.GameObjectsInteractor.HoveredGameObject is not null)
            {
                var obi = mPlanetSystemLayer.GameState.GameObjectsInteractor.HoveredGameObject;
                TextureManager.Instance.DrawAdaptiveCircle(obi.Position, obi.BoundedBox.Radius, Color.White * .4f, 2, 1, mPlanetSystemLayer.Camera2D.Zoom);
            }
            foreach (var spaceship in mPlanetSystemLayer.PlanetsystemState.GameObjects.OfType<Spaceship>())
                TextureManager.Instance.Draw(GameSpriteRegistries.radar, spaceship.Position, .04f / mPlanetSystemLayer.Camera2D.Zoom, 0, spaceship.TextureDepth + 1, spaceship.Fraction == Fractions.Enemys ? Color.Red : Color.LightGreen);
            foreach (var planet in mPlanetSystemLayer.PlanetsystemState.Planets)
                TextureManager.Instance.DrawAdaptiveCircle(Vector2.Zero, planet.Position.Length(), Color.Gray * .1f, 1, planet.TextureDepth - 1, mPlanetSystemLayer.Camera2D.Zoom);
            spriteBatch.End();

            spriteBatch.Begin();
            mMainFrame.Draw();
            spriteBatch.End();
            base.Draw(spriteBatch);
        }
    }
}