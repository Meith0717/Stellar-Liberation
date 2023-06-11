using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Menue.Layers
{
    public class PauseLayer : Layer
    {
        private MyUiFrame mBackgroundLayer;
        private MyUiSprite mContinueButton;
        private MyUiSprite mExitButton;

        public PauseLayer(Game1 game)
            : base(game)
        {
            UpdateBelow = false;

            mBackgroundLayer = new(mGraphicsDevice.Viewport.Width / 2, mGraphicsDevice.Viewport.Height / 2,
                mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height)
            { Alpha = 0.5f, Color = Color.Black};
            mContinueButton = new(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 - 100,
                "buttonContinue")
            {
                OnClickAction = mLayerManager.PopLayer,
                MouseActionType = MouseActionType.LeftClickReleased,
                Color = Color.OrangeRed
            };
            mExitButton = new(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 + 100,
                "buttonExitgame")
            {
                OnClickAction = mLayerManager.Exit,
                MouseActionType = MouseActionType.LeftClickReleased,
                Color = Color.OrangeRed
            };
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mBackgroundLayer.Draw(mTextureManager);
            mContinueButton.Draw(mTextureManager);
            mExitButton.Draw(mTextureManager);
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mBackgroundLayer.OnResolutionChanged(mGraphicsDevice.Viewport.Width / 2, mGraphicsDevice.Viewport.Height / 2,
                mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            mContinueButton.OnResolutionChanged(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 - 100);
            mExitButton.OnResolutionChanged(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 + 100);

        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mContinueButton.Update(mTextureManager, inputState);
            mExitButton.Update(mTextureManager, inputState);
            if (inputState.mActionList.Contains(ActionType.ESC))
            {
                mLayerManager.PopLayer();
            }
        }
    }
}
