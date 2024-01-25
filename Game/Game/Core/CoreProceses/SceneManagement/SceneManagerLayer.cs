// SceneManagerLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
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

        protected SceneManagerLayer() : base(false) { }

        public void AddScene(Scene scene)
        {
            ArgumentNullException.ThrowIfNull(mGraphicsDevice);
            scene.Initialize(mGraphicsDevice);
            Scenes.AddLast(scene);
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
            Scenes.Last().Update(gameTime, inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DebugSystem.UpdateFrameCounting();

            // Update Render Targets of scenes
            foreach (Scene scene in Scenes) scene.Draw(this, spriteBatch);

            spriteBatch.Begin();
            DebugSystem.DrawOnScreen();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            foreach (Scene scene in Scenes) scene.OnResolutionChanged();
        }
    }
}
