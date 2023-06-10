using Galaxy_Explovive.Core.Debug;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface;
using Galaxy_Explovive.Core.UserInterface.UiWidgets;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using Galaxy_Explovive.Menue.Layers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Galaxy_Explovive.Game
{
    public class HudLayer : Layer
    {
        private readonly GameLayer mGameLayer;
        private readonly UiFrame mTopBar;
        private readonly UiFrame mBottomBar;
        private readonly UiText mGameTimeText;

        public HudLayer(Game1 game, GameLayer gameLayer)
            : base(game)
        {
            UpdateBelow = true;
            mGameLayer = gameLayer;

            mTopBar = new(null, mTextureManager, mGraphicsDevice)
            {
                RelativeW = 1f,
                Color = new Color(66, 73, 73),
                Height = 75,
                EdgeWidth = 50,
                MarginY = -20,
                Side = UiCanvas.RootSide.N
            };

            mGameTimeText = new(mTopBar, mTextureManager, mGraphicsDevice) 
            { FontColor = Color.White, Side = UiCanvas.RootSide.W, RelativY = 0.65f, MarginX = 20 };

            mBottomBar = new(null, mTextureManager, mGraphicsDevice)
            { RelativeW = 1, Height = 40, Alpha = 0, Side = UiCanvas.RootSide.S };

            UiFrame leftButtonLayer = new(mBottomBar, mTextureManager, mGraphicsDevice)
            { Height = 40, Width = 200, Alpha = 0, Side = UiCanvas.RootSide.E};

            _ = new UiButton(leftButtonLayer, mTextureManager, mGraphicsDevice, "menueButton", 0.2f)
            { RelativX = .90f, OnKlick = Pause };
            _ = new UiButton(leftButtonLayer, mTextureManager, mGraphicsDevice, "buttonDeselect", 0.2f)
            { RelativX = .50f, OnKlick = Deselect };
            _ = new UiButton(leftButtonLayer, mTextureManager, mGraphicsDevice, "buttonTrack", 0.2f)
            { RelativX = .10f, OnKlick = Track };

            OnResolutionChanged();
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mTopBar.Draw();
            mBottomBar.Draw();
            mGameLayer.mDebugSystem.ShowRenderInfo(mTextureManager,mGameLayer.mCamera.Zoom, mGameLayer.mCamera.Position);
            mGameLayer.mGameMessages.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mTopBar.OnResolutionChanged();
            mBottomBar.OnResolutionChanged();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.ESC))
            {
                Pause();
            }
            mGameTimeText.Text = MyUtility.ConvertSecondsToTimeUnits((int)mGameLayer.GameTime);
            mTopBar.Update(inputState);
            mBottomBar.Update(inputState);
        }

        private void Pause()
        {
            mLayerManager.AddLayer(new PauseLayer(mGame));
        }

        private void Deselect()
        {
            mGameLayer.SelectObject = null;
        }

        private void Track()
        {
            if (mGameLayer.SelectObject is Cargo)
            {
                Cargo ship = (Cargo)mGameLayer.SelectObject;
                ship.mTrack = true;
            }
        }
    }
}
