using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;
using Galaxy_Explovive.Core.UserInterface.Widgets;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Game.Layers
{
    public class PauseLayer : Layer
    {
        private UiLayer mBackgroundLayer;

        public PauseLayer() 
        {
            UpdateBelow = false;

            mBackgroundLayer = new(null) { Color = Color.Black, Alpha = .5f, Fill = Core.UserInterface.UiCanvas.RootFill.Cover };
            _ = new UiButton(mBackgroundLayer, "buttonContinue", 0.05f) { RelativY = 0.4f, OnKlick = Globals.mLayerManager.PopLayer};
            _ = new UiButton(mBackgroundLayer, "buttonExitgame", 0.05f){ RelativY = 0.6f, OnKlick = Globals.mLayerManager.Exit};
            OnResolutionChanged();
        }

        public override void Destroy()
        {
        }

        public override void Draw()
        {
            Globals.mSpriteBatch.Begin();
            mBackgroundLayer.Draw();
            Globals.mSpriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            mBackgroundLayer.OnResolutionChanged();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.ESC))
            {
                Globals.mLayerManager.PopLayer();
            }
            mBackgroundLayer.Update(inputState);
        }
    }
}
