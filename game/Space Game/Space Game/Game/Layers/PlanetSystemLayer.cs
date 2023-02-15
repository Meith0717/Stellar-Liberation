using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.LayerManagement;
using rache_der_reti.Core.Menu;
using rache_der_reti.Core.SoundManagement;
using rache_der_reti.Core.TextureManagement;

namespace Space_Game.Game.Layers
{
    public class PlanetSystemLayer : Layer
    {
        private UiElement mRoot;
        private UiElementList mRootList;
        public PlanetSystemLayer() : base()
        {
            UpdateBelow = true;
            // root item
            mRoot = new UiElement();
            mRoot.BackgroundColor = Color.Black;
            mRoot.BackgroundAlpha = 0.9f;
            mRootList = new UiElementList(false);
            mRootList.Width = (int)(mGraphicsDevice.Viewport.Width * 0.95f);
            mRootList.Height = (int)(mRootList.Width * 0.5f);
            mRoot.ChildElements.Add(mRootList);

            UiElementText text = new UiElementText("Planet System Layer");
            text.FontColor = Color.White;
            mRootList.ChildElements.Add(text);

            OnResolutionChanged();
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            if (inputState.mActionList.Contains(ActionType.Exit))
            {
                Exit();
            }
            mRoot.HandleInput(inputState);
        }
        public override void Draw()
        {
            mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mRoot.Render();
            mSpriteBatch.End();
        }
        public override void Destroy() { }

        public override void OnResolutionChanged()
        {
            mRoot.Update(new Rectangle(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height));
        }

        public void Exit()
        {
            mLayerManager.PopLayer();
        }
    }
}
