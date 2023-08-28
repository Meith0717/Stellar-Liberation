using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CelestialOdyssey.Game.Layers
{
    public class PauseLayer : Layer
    {
        private MainGameLayer mGameLayer;
        private MyUiFrame mBackgroundLayer;
        private MyUiFrame mForegroundLayer;
        private MyUiSprite mContinueButton;
        private MyUiSprite mExitButton;
        private MyUiSprite mExitSaveButton;

        public PauseLayer(MainGameLayer gameLayer)
            : base(true)
        {
            UpdateBelow = false;

            mGameLayer = gameLayer;
            mBackgroundLayer = new(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)
            { Alpha = 0.95f, Color = Color.Black };
            mContinueButton = new(GraphicsDevice.Viewport.Width / 2 - 256, GraphicsDevice.Viewport.Height / 2 - 64 - 100,
                "buttonContinue")
            {
                OnClickAction = LayerManager.PopLayer,
                MouseActionType = MouseActionType.LeftClickReleased,
                Color = Color.OrangeRed
            };
            mExitButton = new(GraphicsDevice.Viewport.Width / 2 - 256, GraphicsDevice.Viewport.Height / 2 - 64 + 100,
                "buttonExitgame")
            {
                OnClickAction = ExitAndSave,
                MouseActionType = MouseActionType.LeftClickReleased,
                Color = Color.OrangeRed
            };
            mExitSaveButton = new(GraphicsDevice.Viewport.Width / 2 - 256, GraphicsDevice.Viewport.Height / 2 - 64 + 200,
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
            mBackgroundLayer.OnResolutionChanged(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            mContinueButton.OnResolutionChanged(GraphicsDevice.Viewport.Width / 2 - 256, GraphicsDevice.Viewport.Height / 2 - 64 - 100);
            mExitButton.OnResolutionChanged(GraphicsDevice.Viewport.Width / 2 - 256, GraphicsDevice.Viewport.Height / 2 - 64 + 100);
            mExitSaveButton.OnResolutionChanged(GraphicsDevice.Viewport.Width / 2 - 256, GraphicsDevice.Viewport.Height / 2 - 64 + 200);

        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mContinueButton.Update(inputState);
            mExitButton.Update(inputState);
            mExitSaveButton.Update(inputState);
            if (inputState.mActionList.Contains(ActionType.ESC))
            {
                LayerManager.PopLayer();
            }
        }

        private void ExitAndSave()
        {
            LayerManager.Exit();
        }

        private void Exit()
        {
            LayerManager.Exit();
        }
    }
}
