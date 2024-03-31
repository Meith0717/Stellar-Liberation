// PlanetSystemLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.ParallaxSystem;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class PlanetSystemLayer : GameLayer
    {
        public PlanetSystem PlanetSystem { get; private set; }
        private readonly UiFrame mBackgroundLayer;
        private readonly ParallaxController mParallaxController;

        public PlanetSystemLayer(GameLayerManager gameState, PlanetSystem planetSystem, Game1 game1)
            : base(gameState, 5000, game1)
        {
            HUDLayer = new PlanetSystemHud(this, game1);
            HUDLayer.ApplyResolution();

            mBackgroundLayer = new()
            {
                Color = Color.Black,
                Anchor = Anchor.Center,
                FillScale = FillScale.FillIn
            };

            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground)
            {
                Anchor = Anchor.Center,
                FillScale = FillScale.FillIn
            });

            Camera2D.Zoom = 0.01f;
            PlanetSystem = planetSystem;
            mParallaxController = new(Camera2D);
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax1, .01f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax2, .04f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax3, .08f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax4, .12f));
            ApplyResolution();
        }

        public void OpenMap() => GameState.AddLayer(new MapLayer(GameState, PlanetSystem, Game1));

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mParallaxController.Update();
            mBackgroundLayer.Update(inputState, gameTime);
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .008f, 1);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(gameTime, inputState, Camera2D);
            GameState.GameObjectsInteractor.Update(inputState, this, HUDLayer, PlanetSystem);
            GameObject2DManager.Update(gameTime, inputState, this, ref PlanetSystem.GameObjects);
            base.Update(gameTime, inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mBackgroundLayer.Draw();
            mParallaxController.Draw();
            spriteBatch.End();

            base.Draw(spriteBatch);

            spriteBatch.Begin(transformMatrix: ViewTransformationMatrix);
            GameState.GameObjectsInteractor.Draw(Camera2D);
            spriteBatch.End();
        }


        public override void ApplyResolution() { mBackgroundLayer.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution); }

        public override void Destroy() {; }
    }
}
