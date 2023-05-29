using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface.UiWidgets;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaxy_Explovive.Menue.Layers
{
    public class PauseLayer : Layer
    {
        private UiFrame mBackgroundLayer;

        public PauseLayer(Game1 game)
            : base(game)
        {
            UpdateBelow = false;

            mBackgroundLayer = new(null, mTextureManager, mGraphicsDevice) 
                { Color = Color.Black, Alpha = .5f, Fill = UiCanvas.RootFill.Cover };
            _ = new UiButton(mBackgroundLayer, mTextureManager, mGraphicsDevice, "buttonContinue", 0.05f) 
                { RelativY = 0.4f, OnKlick = mLayerManager.PopLayer };
            _ = new UiButton(mBackgroundLayer, mTextureManager, mGraphicsDevice, "buttonExitgame", 0.05f) 
                { RelativY = 0.6f, OnKlick = mLayerManager.Exit };
            OnResolutionChanged();
        }

        public override void Destroy()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            mBackgroundLayer.Draw();
            spriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mBackgroundLayer.OnResolutionChanged();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.ESC))
            {
                mLayerManager.PopLayer();
            }
            mBackgroundLayer.Update(inputState);
        }
    }
}
