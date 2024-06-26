﻿// PlanetSystemLayer.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.UserInterface;
using StellarLiberation.Game.Core.Visuals.ParallaxSystem;
using StellarLiberation.Game.Core.Visuals.Rendering;

namespace StellarLiberation.Game.Layers.GameLayers
{
    internal class PlanetsystemLayer : GameLayer
    {
        public readonly PlanetsystemState PlanetsystemState;
        private readonly ParallaxController mParallaxController;
        private readonly PlanetSystemHudLayer mHud;

        public PlanetsystemLayer(GameState gameState, PlanetsystemState planetsystemState, Game1 game1)
            : base(gameState, planetsystemState.SpatialHashing, game1)
        {
            mHud = new(gameState, this, game1);
            PlanetsystemState = planetsystemState;

            var background = new UiSprite("gameBackground")
            {
                Anchor = Anchor.Center,
                FillScale = FillScale.FillIn
            };

            AddUiElement(background);

            Camera2D.Zoom = 0.01f;
            mParallaxController = new(Camera2D);
            mParallaxController.Add(new("gameBackgroundParlax1", .01f));
            mParallaxController.Add(new("gameBackgroundParlax2", .04f));
            mParallaxController.Add(new("gameBackgroundParlax3", .08f));
            mParallaxController.Add(new("gameBackgroundParlax4", .12f));
            ApplyResolution();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            inputState.DoAction(ActionType.ToggleHyperMap, GameState.OpenMap);
            mParallaxController.Update();
            mHud.Update(gameTime, inputState);
            Camera2DMover.ControllZoom(gameTime, inputState, Camera2D, .008f, 1);
            Camera2DMover.UpdateCameraByMouseDrag(inputState, Camera2D);
            Camera2DMover.MoveByKeys(gameTime, inputState, Camera2D);
            GameState.GameObjectsInteractor.Update(inputState, PlanetsystemState, WorldMousePosition, GameState);
            mParticleManager.Update(gameTime, PlanetsystemState.ParticleEmitors);
            mStereoSoundSystem.Update(Camera2D.Position, Camera2D.Zoom, PlanetsystemState.StereoSounds);
            base.Update(gameTime, inputState);
        }

        public override void ApplyResolution()
        {
            base.ApplyResolution();
            mHud.ApplyResolution();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mParallaxController.Draw();
            spriteBatch.End();

            base.Draw(spriteBatch);

            spriteBatch.Begin(transformMatrix: ViewTransformationMatrix);
            GameState.GameObjectsInteractor.Draw(Camera2D);
            spriteBatch.End();

            mHud.Draw(spriteBatch);

            PlanetsystemState.StereoSounds.Clear();
            PlanetsystemState.ParticleEmitors.Clear();
        }
    }
}
