// SceneManagerLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.LayerManagement
{
    [Serializable]
    public abstract class SceneManagerLayer : Layer
    {
        public readonly Debugger.DebugSystem DebugSystem = new();
        protected readonly LinkedList<Scene> Scenes = new();

        protected SceneManagerLayer() : base(false) { }

        public void AddScene(Scene scene)
        {
            Scenes.AddLast(scene);
        }

        public void PopScene()
        {
            Scenes.RemoveLast();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            DebugSystem.Update(gameTime, inputState);
            if (Scenes.Any()) Scenes.Last().Update(gameTime, inputState, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DebugSystem.UpdateFrameCounting();

            if (Scenes.Any()) Scenes.Last().Draw(this, spriteBatch);

            spriteBatch.Begin();
            DebugSystem.DrawOnScreen();
            spriteBatch.End();
        }
    }
}
