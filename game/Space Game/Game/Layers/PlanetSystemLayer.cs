using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.LayerManagement;
using rache_der_reti.Core.Menu;
using rache_der_reti.Core.SoundManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Game.GameObjects;
using System.Runtime.Intrinsics.X86;
using static rache_der_reti.Core.Menu.UiElement;

namespace Space_Game.Game.Layers
{
    public class PlanetSystemLayer : Layer
    {
        private UiElement mRoot;
        private UiElementList mRootList;
        private PlanetSystem mPlanetSystem;


        public PlanetSystemLayer(PlanetSystem planetSystem) : base()
        {
            UpdateBelow = true;
            mPlanetSystem = planetSystem;
        
            // root item
            mRoot = new UiElement();
            mRoot.BackgroundColor = Color.Black;
            mRoot.BackgroundAlpha = 1f;
            mRootList = new UiElementList(false);
            mRootList.Width = (int)(mGraphicsDevice.Viewport.Width);
            mRootList.Height = (int)(mGraphicsDevice.Viewport.Height);
            mRoot.ChildElements.Add(mRootList);

            UiElementText text = new UiElementText("Planet System Layer", "title");
            text.MyHorizontalAlignt = UiElement.HorizontalAlignment.Center;
            text.MyVerticalAlignment = UiElement.VerticalAlignment.Top;
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
            foreach (Planet planet in mPlanetSystem.mPlanetList)
            {
                planet.Update(gameTime, inputState);
            }
            mRoot.HandleInput(inputState);
        }
        public override void Draw()
        {
            Vector2 heightCenter = new Vector2(0, mGraphicsDevice.Viewport.Height / 2);

            mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mRoot.Render();
            TextureManager.GetInstance().Draw(mPlanetSystem.TextureId,
                heightCenter - mPlanetSystem.Offset, mPlanetSystem.TextureWidth, mPlanetSystem.TextureHeight);
            foreach (Planet planet in mPlanetSystem.mPlanetList)
            {
                planet.Draw();
            }
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
