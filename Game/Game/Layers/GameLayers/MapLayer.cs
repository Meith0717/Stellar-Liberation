// MapLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.Layers.MenueLayers;
using System.Collections.Generic;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class MapLayer : GameLayer
    {
        private readonly UiFrame mBackgroundLayer;

        private readonly List<PlanetSystem> mPlanetSystems;
        private readonly PlanetSystem mCurrentSystem;

        public MapLayer(GameState gameLayer, List<PlanetSystem> planetSystems, PlanetSystem currentSystem)
            : base(gameLayer, MapFactory.MapScale * 3)
        {
            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn, Alpha = 1 };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            mCurrentSystem = currentSystem;
            Camera2D.Position = mCurrentSystem.Position;
            mPlanetSystems = planetSystems;
            foreach (var system in mPlanetSystems)
            {
                SpatialHashing.InsertObject(system, (int)system.Position.X, (int)system.Position.Y);
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ESC, () => LayerManager.AddLayer(new PauseLayer()));

            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, 0.1f, 5);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(inputState, Camera2D);

            mBackgroundLayer.Update(inputState, mGraphicsDevice.Viewport.Bounds, 1);
            inputState.DoAction(ActionType.ToggleHyperMap, LayerManager.PopLayer);
            foreach (var system in mPlanetSystems) system.Update(gameTime, inputState, this);
            base.Update(gameTime, inputState);
        }

        public override void DrawOnScreenView(GameState gameState, SpriteBatch spriteBatch)
        {
            mBackgroundLayer.Draw();
        }

        public override void DrawOnWorldView(GameState gameState, SpriteBatch spriteBatch) {; }

        public override void OnResolutionChanged() { base.OnResolutionChanged(); }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new System.NotImplementedException();
        }

        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }

    }
}
