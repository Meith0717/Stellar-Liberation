// PlanetSystemLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.ParallaxSystem;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class PlanetSystemLayer : GameLayer
    {
        public readonly PlanetSystem PlanetSystem;
        private readonly UiFrame mBackgroundLayer;
        private readonly ParallaxController mParallaxController;
        public readonly GameObjectsInteractor SpaceShipInteractor;

        public PlanetSystemLayer(GameLayerManager gameState, PlanetSystem planetSystem, Game1 game1)
            : base(gameState, 10000, game1)
        {
            HUDLayer = new PlanetSystemHud(this, game1);
            HUDLayer.ApplyResolution();
            SpaceShipInteractor = new();

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
            GameObjectsManager.AddRange(PlanetSystem.AstrononomicalObjects);
            GameObjectsManager.AddRange(GameState.SpaceShips.GetSpaceshipsOfPlanetSystem(PlanetSystem));
            GameObjectsManager.Initialize(SpatialHashing);

            mParallaxController = new(Camera2D);
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax1, .01f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax2, .04f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax3, .08f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax4, .12f));
            ApplyResolution();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameState.AddLayer(new MapLayer(GameState, PlanetSystem, Game1)));

            mParallaxController.Update();
            mBackgroundLayer.Update(inputState, gameTime);
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .008f, 1);
            Camera2DMover.EdgeScrolling(inputState, gameTime, GraphicsDevice, Camera2D);
            Camera2DMover.MoveByKeys(gameTime, inputState, Camera2D);
            SpaceShipInteractor.Update(inputState, this);
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
            SpaceShipInteractor.Draw(Camera2D);
            spriteBatch.End();
        }


        public override void ApplyResolution() { mBackgroundLayer.ApplyResolution(GraphicsDevice.Viewport.Bounds, ResolutionManager.Resolution); }

        public override void Destroy() {; }
    }
}
