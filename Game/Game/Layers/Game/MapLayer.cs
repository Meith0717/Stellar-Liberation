﻿// MapLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.Objects.UiElements;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.Rendering;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class MapLayer : GameLayer
    {
        private readonly UiFrame mBackgroundLayer;
        private readonly PlanetSystem mCurrentSystem;

        public MapLayer(GameLayerManager gameState, PlanetSystem currentSystem, Game1 game1)
            : base(gameState, MapFactory.MapScale * 3, game1)
        {
            mBackgroundLayer = new() { Color = Color.Black, Anchor = Anchor.Center, FillScale = FillScale.FillIn, Alpha = 1 };
            mBackgroundLayer.AddChild(new UiSprite(GameSpriteRegistries.gameBackground) { Anchor = Anchor.Center, FillScale = FillScale.FillIn });

            Camera2D.Position = currentSystem.Position;
            mCurrentSystem = currentSystem;
            foreach (var system in gameState.PlanetSystems)
            {
                SpatialHashing.InsertObject(system, (int)system.Position.X, (int)system.Position.Y);
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, 0.01f, 5);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(gameTime, inputState, Camera2D);

            mBackgroundLayer.Update(inputState, gameTime);
            inputState.DoAction(ActionType.ToggleHyperMap, GameState.PopLayer);
            foreach (var system in GameState.PlanetSystems)
            {
                system.Update(gameTime, inputState, this);
            }
            var objByMouse = SpatialHashing.GetObjectsInRadius<PlanetSystem>(WorldMousePosition, 200);
            foreach (var system in objByMouse)
            {
                GameObject2DInteractionManager.Manage(inputState, system, this, () => LeftPressAction(system), null, () => system.IsHovered = true); ;
            }

            base.Update(gameTime, inputState);
        }

        private void LeftPressAction(PlanetSystem planetSystem)
        {
            if (mCurrentSystem == planetSystem) return;
            GameState.PopLayer();
            GameState.AddLayer(new PlanetSystemLayer(GameState, planetSystem, Game1));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mBackgroundLayer.Draw();
            spriteBatch.End();
            base.Draw(spriteBatch);
        }

        public override void ApplyResolution() { base.ApplyResolution(); }

        public override void Destroy() { }
    }
}
