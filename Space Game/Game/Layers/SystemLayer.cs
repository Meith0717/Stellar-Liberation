using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CelestialOdyssey.Game.Layers
{
    [Serializable]
    public class SystemLayer : GameLayer
    {
        private readonly ParllaxManager mParllaxManager;

        public SystemLayer() : base(10000)
        {
            Camera.SetPosition(Vector2.Zero);
            Camera.SetZoom(0.005f);
            mParllaxManager = new();
            mParllaxManager.Add(new(ContentRegistry.gameBackground.Name, 0.05f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax.Name, 0.1f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax1.Name, 0.15f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax2.Name, 0.2f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax3.Name, 0.25f));

        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            mParllaxManager.Update(Camera.Movement, Camera.Zoom);
            if (inputState.mActionList.Contains(ActionType.Map)) { LayerManager.PopLayer(); }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            spriteBatch.End();

            base.Draw(spriteBatch);
        }

        public override void Destroy() { }

        public override void DrawOnWorld() { }

        public override void OnResolutionChanged()
        {
            mParllaxManager.OnResolutionChanged(GraphicsDevice);
        }
    }
}
