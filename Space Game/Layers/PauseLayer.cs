using GalaxyExplovive.Core.GameEngine.InputManagement;
using GalaxyExplovive.Core.LayerManagement;
using GalaxyExplovive.Core.UserInterface;
using GalaxyExplovive.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GalaxyExplovive.Layers
{
    public class PauseLayer : Layer
    {
        private GameState Game;
        private MyUiFrame mBackgroundLayer;
        private MyUiFrame mForegroundLayer;
        private MyUiSprite mContinueButton;
        private MyUiSprite mExitButton;
        private MyUiSprite mExitSaveButton;

        public PauseLayer(Game1 app, GameState game)
            : base(app)
        {
            UpdateBelow = false;

            Game = game;
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
                OnClickAction = ExitAndSave,
                MouseActionType = MouseActionType.LeftClickReleased,
                Color = Color.OrangeRed
            };
            mExitSaveButton = new(mGraphicsDevice.Viewport.Width / 2 - 256, mGraphicsDevice.Viewport.Height / 2 - 64 + 200,
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
            mBackgroundLayer.Draw(mTextureManager);
            mContinueButton.Draw(mTextureManager);
            mExitButton.Draw(mTextureManager);
            mExitSaveButton.Draw(mTextureManager);
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
            mContinueButton.Update(mTextureManager, inputState);
            mExitButton.Update(mTextureManager, inputState);
            mExitSaveButton.Update(mTextureManager, inputState);
            if (inputState.mActionList.Contains(ActionType.ESC))
            {
                mLayerManager.PopLayer();
            }
        }

        private void ExitAndSave()
        {
            mSerialize.SerializeObject(Game, "save");
            mLayerManager.Exit();
        }

        private void Exit()
        {
            mLayerManager.Exit();
        }
    }
}
