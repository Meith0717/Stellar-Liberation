using Galaxy_Explovive.Core.UserInterface.Widgets;

namespace Galaxy_Explovive.Core.UserInterface
{
    public class MessageText
    {
        const float liveTime = 5;
        public string Message { get; private set; }
        private float mCreatedGameTime;

        public MessageText(string text, float gameTime)
        {
            Message = text;
            mCreatedGameTime = gameTime;
        }

        public bool TimeElapsed(float gameTime)
        {
            return mCreatedGameTime + liveTime <= gameTime;
        }
    }
}
