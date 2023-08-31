using CelestialOdyssey.Game;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CelestialOdyssey.Game.Layers
{
    public class PauseLayer : Layer
    {
        private MyUiFrame mBackgroundLayer;
        private MyUiFrame mForegroundLayer;
        private MyUiSprite mContinueButton;
        private MyUiSprite mExitButton;
        private MyUiSprite mExitSaveButton;

        public PauseLayer()
            : base(false)
        {

            mBackgroundLayer = new(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height)
            { Alpha = 0.95f, Color = Color.Black };
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
                OnClickAction = Exit,
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
            mBackgroundLayer.Draw();
            mContinueButton.Draw();
            mExitButton.Draw();
            mExitSaveButton.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mBackgroundLayer.OnResolutionChanged(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            mContinueButton.OnResolutionChanged(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 - 100);
            mExitButton.OnResolutionChanged(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 + 100);
            mExitSaveButton.OnResolutionChanged(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 + 200);

        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mContinueButton.Update(inputState);
            mExitButton.Update(inputState);
            mExitSaveButton.Update(inputState);
            if (inputState.mActionList.Contains(ActionType.ESC))
            {
                mLayerManager.PopLayer();
            }
        }

        private void Exit()
        {
            mLayerManager.Exit();
        }
    }
}
