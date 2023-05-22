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
            UiLayer frame = new(mBackgroundLayer) { Color = new Color(63, 63, 63), EdgeWidth = 30};
            _ = new UiButton(frame, "buttonContinue", 0.2f) { RelativY = 0.25f, OnKlick = Globals.mLayerManager.PopLayer};
            _ = new UiButton(frame, "buttonExitgame", 0.2f){ RelativY = 0.75f, OnKlick = Globals.mLayerManager.Exit};
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
            mBackgroundLayer.Update(inputState);
        }
    }
}
