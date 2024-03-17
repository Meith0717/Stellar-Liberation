// PlanetSystemLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.GridSystem;
using StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components;
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
        private readonly SpaceshipController spaceShipController = new();

        private readonly UiFrame mBackgroundLayer;
        private readonly ParallaxController mParallaxController;
        private readonly Grid mGrid;

        public PlanetSystemLayer(GameLayerManager gameState, PlanetSystem planetSystem, float camZoom) 
            : base(gameState, 25000)
        {
            mGrid = new(10000);
            HUDLayer = new HudLayer(this);

            mBackgroundLayer = new() { 
                Color = Color.Black, 
                Anchor = Anchor.Center, 
                FillScale = FillScale.FillIn 
            };

            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { 
                Anchor = Anchor.Center, 
                FillScale = FillScale.FillIn
            });

            Camera2D.Zoom = camZoom;
            PlanetSystem = planetSystem;
            GameObjects.AddRange(PlanetSystem.AstrononomicalObjects);
            GameObjects.AddRange(GameState.SpaceShips.GetSpaceshipsOfPlanetSystem(PlanetSystem));

            mParallaxController = new(Camera2D);
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax1, .01f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax2, .04f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax3, .08f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax4, .12f));
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameState.AddLayer(new MapLayer(GameState, PlanetSystem)));

            mParallaxController.Update();
            mBackgroundLayer.Update(inputState, gameTime, GraphicsDevice.Viewport.Bounds, 1);
            spaceShipController.Controll(gameTime, GameState.Player, inputState, this);
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .001f, 1);
            base.Update(gameTime, inputState);
        }

        public override void DrawOnScreenView(SpriteBatch spriteBatch)
        {
            mBackgroundLayer.Draw();
            mParallaxController.Draw();
        }

        public override void DrawOnWorldView(SpriteBatch spriteBatch) => mGrid.Draw(this);

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }

        public override void Destroy() {; }
    }
}
