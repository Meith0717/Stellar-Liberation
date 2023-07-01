using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.InputManagement;
using GalaxyExplovive.Core.LayerManagement;
using GalaxyExplovive.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GalaxyExplovive.Layers
{
    internal class TestLayer : Layer
    {
        private readonly GameState mGameState;

        public TestLayer(Game1 app) : base(app)
        {
            GameEngine engine = new(mSoundManager, new(mTextureManager, mGraphicsDevice.Viewport.Width / 2, 50));
            mGameState = new(engine);
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mGameState.Draw(spriteBatch, mTextureManager);
        }

        public override void OnResolutionChanged()
        {
            mGameState.ApplyResolution(mGraphicsDevice);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mGameState.Update(inputState, gameTime, mGraphicsDevice);
        }
    }
}
