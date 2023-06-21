using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface;
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
        MyUiFrame TopFrame;
        MyUiText GameTime;
        MyUiSprite MenuButton;

        public TestLayer(Game1 game) : base(game)
        {
            TopFrame = new(mGraphicsDevice.Viewport.Width / 2, 15, mGraphicsDevice.Viewport.Width, 30)
            {
                Color = Color.DarkOliveGreen,
                Alpha = 0.5f
            };
            GameTime = new(2, 2, "Time");
            MenuButton = new(mGraphicsDevice.Viewport.Width - 60, mGraphicsDevice.Viewport.Height - 40, "menueButton")
            { Color = Color.DarkOliveGreen, Scale = 0.5f };
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            TopFrame.Draw(mTextureManager);
            GameTime.Draw(mTextureManager);
            MenuButton.Draw(mTextureManager);
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            TopFrame.OnResolutionChanged(mGraphicsDevice.Viewport.Width / 2, 15, mGraphicsDevice.Viewport.Width, 30);
            MenuButton.OnResolutionChanged(mGraphicsDevice.Viewport.Width - 60, mGraphicsDevice.Viewport.Height - 40);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            MenuButton.Update(mTextureManager, inputState);
        }
    }
}
