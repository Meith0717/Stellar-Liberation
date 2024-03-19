// PlanetSystemLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private readonly UiFrame mBackgroundLayer;
        private readonly ParallaxController mParallaxController;
        private readonly Grid mGrid;

        public PlanetSystemLayer(GameLayerManager gameState, PlanetSystem planetSystem, Game1 game1) 
            : base(gameState, 25000, game1)
        {
            mGrid = new(10000);
            HUDLayer = new HudLayer(this, game1);

            mBackgroundLayer = new() { 
                Color = Color.Black, 
                Anchor = Anchor.Center, 
                FillScale = FillScale.FillIn 
            };

            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { 
                Anchor = Anchor.Center, 
                FillScale = FillScale.FillIn
            });

            Camera2D.Zoom = 0.01f;
            PlanetSystem = planetSystem;
            GameObjects.AddRange(PlanetSystem.AstrononomicalObjects);
            GameObjects.AddRange(GameState.SpaceShips.GetSpaceshipsOfPlanetSystem(PlanetSystem));
            GameObject2DManager.Initialize(SpatialHashing, ref GameObjects, this);

            mParallaxController = new(Camera2D);
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax1, .01f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax2, .04f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax3, .08f));
            mParallaxController.Add(new(GameSpriteRegistries.gameBackgroundParlax4, .12f));
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameState.AddLayer(new MapLayer(GameState, PlanetSystem, Game1)));

            mParallaxController.Update();
            mBackgroundLayer.Update(inputState, gameTime);
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .0001f, 1);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            base.Update(gameTime, inputState);
        }

        public override void DrawOnScreenView(SpriteBatch spriteBatch)
        {
            mBackgroundLayer.Draw();
            mParallaxController.Draw();
        }

        public override void DrawOnWorldView(SpriteBatch spriteBatch) => mGrid.Draw(this);

        public override void ApplyResolution() { base.ApplyResolution(); }

        public override void Destroy() {; }
    }
}
