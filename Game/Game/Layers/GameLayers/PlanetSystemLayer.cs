// PlanetSystemLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.GridSystem;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using System.Collections.Generic;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class PlanetSystemLayer : GameLayer
    {
        private readonly GameObject2DManager mUnsavedObjects;
        private readonly GameObject2DManager mSavedObjects;

        private readonly UiFrame mBackgroundLayer;
        private readonly Grid mGrid;

        public PlanetSystemLayer(GameStateLayer gameState, List<GameObject2D> unsavedObjects, List<GameObject2D> savedObjects, float camZoom) : base(gameState, 50000)
        {
            mUnsavedObjects = new(unsavedObjects, this, SpatialHashing);
            mSavedObjects = new(savedObjects, this, SpatialHashing);

            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            HUDLayer = new HudLayer(this);

            Camera2D.Zoom = camZoom;
            mGrid = new(10000);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, () => GameState.AddLayer(new MapLayer(GameState)));
            inputState.DoAction(ActionType.Inventar, () => LayerManager.AddLayer(new InventoryLayer(GameState.Inventory, GameState.Wallet)));

            mBackgroundLayer.Update(inputState, GraphicsDevice.Viewport.Bounds, 1);

            mUnsavedObjects.Update(gameTime, inputState, this);
            mSavedObjects.Update(gameTime, inputState, this);

            GameState.Player.Update(gameTime, inputState, this);
            //GameState.DebugSystem.CheckForSpawn(GameState.CurrentSystem.GetAstronomicalObjects(this, SpatialHashing), this);

            Camera2D.Position = GameState.Player.Position;
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .001f, 1);
            base.Update(gameTime, inputState);
        }

        public override void DrawOnScreenView(GameStateLayer gameState, SpriteBatch spriteBatch) => mBackgroundLayer.Draw();

        public override void DrawOnWorldView(GameStateLayer gameState, SpriteBatch spriteBatch)
        {
            mGrid.Draw(this);
            //TextureManager.Instance.Draw(GameSpriteRegistries.starLightAlpha.Name, Vector2.Zero, mStar.TextureOffset, 300f, 0, 3, mStar.TextureColor);
        }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }

        public override void Destroy() {; }
    }
}
