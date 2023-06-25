using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface;
using Galaxy_Explovive.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Explovive.Layers
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
