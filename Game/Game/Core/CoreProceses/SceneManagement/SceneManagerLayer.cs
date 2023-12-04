// SceneManagerLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.CoreProceses.SceneManagement
{
    [Serializable]
    public abstract class SceneManagerLayer : Layer
    {
        public readonly Debugger.DebugSystem DebugSystem = new();
        protected readonly LinkedList<Scene> Scenes = new();
        private RenderTarget2D mRenderTarget;

        protected SceneManagerLayer() : base(false) { }

        public void AddScene(Scene scene)
        {
            ArgumentNullException.ThrowIfNull(mGraphicsDevice);
            scene.Initialize(mGraphicsDevice);
            Scenes.AddLast(scene);
        }

        public override void Initialize(Game1 game1, LayerManager layerManager, GraphicsDevice graphicsDevice, Serialize serialize)
        {
            base.Initialize(game1, layerManager, graphicsDevice, serialize);
            mRenderTarget = new(graphicsDevice, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
        }

        public void PopScene()
        {
            if (!Scenes.Any()) return;
            Scenes.RemoveLast();
        }

        public void RemoveScene(Scene scene) => Scenes.Remove(scene);

        public override void Update(GameTime gameTime, InputState inputState)
        {
            DebugSystem.Update(gameTime, inputState);
            foreach (Scene scene in Scenes.Reverse()) scene.Update(gameTime, inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DebugSystem.UpdateFrameCounting();

            // Update Render Targets of scenes
            foreach (Scene scene in Scenes) scene.UpdateRenderTarget2D(this, spriteBatch);

            mGraphicsDevice.SetRenderTarget(mRenderTarget);
            mGraphicsDevice.Clear(Color.Transparent);

            // Draw Scene Render Targets on Main Render Target
            spriteBatch.Begin();
            foreach (Scene scene in Scenes) spriteBatch.Draw(scene.RenderTarget, scene.RenderRectangle.ToRectangle(), Color.White);
            spriteBatch.End();
            mGraphicsDevice.SetRenderTarget(null);

            // Draw the RenderTarget onto the screen
            spriteBatch.Begin();
            spriteBatch.Draw(mRenderTarget, mGraphicsDevice.Viewport.Bounds, Color.White);
            spriteBatch.End();

            spriteBatch.Begin();
            DebugSystem.DrawOnScreen();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mRenderTarget.Dispose();
            mRenderTarget = new(mGraphicsDevice, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            foreach (Scene scene in Scenes) scene.OnResolutionChanged();
        }
    }
}
