using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.UserInterface.UiWidgets;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Galaxy_Explovive.Core.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Game.Layers
{
    public class HudLayer : Layer
    {
        private UiFrame mTopBar;
        private UiFrame mBottomBar;
        private UiText mGameTimeText;

        public HudLayer(LayerManager layerManager, SoundManager soundManager, TextureManager textureManager) 
            : base(layerManager, soundManager, textureManager)
        {
            UpdateBelow = true;

            mTopBar = new(null, mTextureManager)
            {
                RelativeW = 1,
                Color = Color.Black,
                Alpha = 0.5f,
                Height = 40,
                Side = UiCanvas.RootSide.Top
            };

            mGameTimeText = new(mTopBar, textureManager) { FontColor = Color.White};

            mBottomBar = new(null, mTextureManager)
            {
                RelativeW = 1,
                Height = 40,
                Alpha = 0,
                Side = UiCanvas.RootSide.Bottom
            };

            UiFrame leftButtonLayer = new(mBottomBar, textureManager)
            {
                Height = 40, Width = 200,
                Alpha = 0,
                Side = UiCanvas.RootSide.Right
            };

            new UiButton(leftButtonLayer, mTextureManager, "menueButton", 0.2f)
            {
                RelativX = .90f,
                OnKlick = Pause
            };

            OnResolutionChanged();
        }



        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mTopBar.Draw();
            mBottomBar.Draw();
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
            mGameTimeText.Text = MyUtility.ConvertSecondsToTimeUnits((int)Globals.GameLayer.GameTime);
            mTopBar.Update(inputState);
            mBottomBar.Update(inputState);
        }

        private void Pause()
        {
            mLayerManager.AddLayer(new PauseLayer(mLayerManager, mSoundManager, mTextureManager));
        }
    }
}
