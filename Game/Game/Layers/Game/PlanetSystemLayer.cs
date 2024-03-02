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
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class PlanetSystemLayer : GameLayer
    {
        private readonly PlanetSystem mPlanetSystem;
        private readonly GameObject2DManager mUnsavedObjects;
        private readonly GameObject2DManager mSavedObjects;

        private readonly UiFrame mBackgroundLayer;
        private readonly Grid mGrid;

        public PlanetSystemLayer(GameLayerManager gameState, PlanetSystem planetSystem, float camZoom) 
            : base(gameState, 25000)
        {
            mPlanetSystem = planetSystem;
            mUnsavedObjects = new(mPlanetSystem.GetAstronomicalObjects(), this, SpatialHashing);
            mSavedObjects = new(mPlanetSystem.GameObjects, this, SpatialHashing);

            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            HUDLayer = new HudLayer(this);

            Camera2D.Zoom = camZoom;
            mGrid = new(10000);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameState.AddLayer(new MapLayer(GameState)));

            mBackgroundLayer.Update(inputState, gameTime, GraphicsDevice.Viewport.Bounds, 1);

            mSavedObjects.Update(gameTime, inputState, this);
            mUnsavedObjects.Update(gameTime, inputState, this);

            Camera2DMover.MoveByKeys(inputState, Camera2D);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .001f, 1);
            base.Update(gameTime, inputState);
        }

        public override void DrawOnScreenView(SpriteBatch spriteBatch) => mBackgroundLayer.Draw();

        public override void DrawOnWorldView(SpriteBatch spriteBatch)
        {
            mGrid.Draw(this);
            //TextureManager.Instance.Draw(GameSpriteRegistries.starLightAlpha.Name, Vector2.Zero, mStar.TextureOffset, 300f, 0, 3, mStar.TextureColor);
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }

        public override void Destroy() {; }
    }
}
