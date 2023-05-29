using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.UserInterface.UiWidgets
{
    public class UiMessages : UiText
    {
        public  float StartGameTime { get; private set; }
        private readonly List<MessageText> mMessages = new();

        public UiMessages(TextureManager textureManager, GraphicsDevice graphicsDevice, float currentGameTime) 
            : base(null, textureManager, graphicsDevice)
        {
            StartGameTime = currentGameTime;
            FontColor = new Color(246, 252, 117);
            Text = "";
            Side = UiCanvas.RootSide.SW;
            MarginX = MarginY = 10;
        }

        public void AddMessage(string message, float gameTime)
        {
            mMessages.Add(new(message, gameTime));
        }

        public void Update(InputState inputState, float gameTime)
        {
            Text = "";
            List<int> l = new();
            for (int i = 0; i < mMessages.Count; i++)
            {
                MessageText message = mMessages[i];
                if (message.TimeElapsed(gameTime))
                {
                    l.Add(i);
                    continue;
                }
                Text += $"{message.Message}\n";
            }

            foreach (int i in l) { mMessages.RemoveAt(i); }

            base.Update(inputState);
        }

        public override void OnResolutionChanged()
        {
            base.OnResolutionChanged();
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
