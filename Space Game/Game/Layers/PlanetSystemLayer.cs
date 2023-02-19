using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.LayerManagement;
using rache_der_reti.Core.Menu;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Game.GameObjects;

namespace Space_Game.Game.Layers
{
    public class PlanetSystemLayer : Layer
    {

        private UiElement mRoot;
        private UiElementList mRootList;
        private PlanetSystem mPlanetSystem;
        private UiElementSprite mBackground;

        public PlanetSystemLayer(PlanetSystem planetSystem) : base()
        {
            UpdateBelow = false;
            mPlanetSystem = planetSystem;

            mBackground = new UiElementSprite("gameBackground");
            mBackground.mSpriteFit = UiElementSprite.SpriteFit.Cover;

            // root item
            mRoot = new UiElement();
            mRoot.BackgroundColor = Color.Black;
            mRoot.BackgroundAlpha = 0.4f;
            mRoot.Width = mGraphicsDevice.Viewport.Width;
            mRoot.Height = mGraphicsDevice.Viewport.Height;
            mRootList = new UiElementList(false);            mRoot.ChildElements.Add(mRootList);

            // Text item
            UiElementText text = new UiElementText("Planet System Layer", "title");
            text.MyHorizontalAlignt = UiElement.HorizontalAlignment.Center;
            text.MyVerticalAlignment = UiElement.VerticalAlignment.Top;
            text.FontColor = Color.White;
            mRootList.ChildElements.Add(text);

            OnResolutionChanged();
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            // Get Inputs
            if (inputState.mActionList.Contains(ActionType.Exit))
            {
                Exit();
            }

            // Update Planets inside Planet System
            foreach (Planet planet in mPlanetSystem.mPlanetList)
            {
                planet.Update(gameTime, inputState);
            }
            mRoot.HandleInput(inputState);
        }

        public override void Draw()
        {
            mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mBackground.Render();
            // Render UiElements
            mRoot.Render();

            // Draw Star
            Vector2 heightCenter = new Vector2(0, mGraphicsDevice.Viewport.Height / 2);
            TextureManager.GetInstance().Draw(mPlanetSystem.TextureId,
                heightCenter - mPlanetSystem.Offset, mPlanetSystem.TextureWidth, mPlanetSystem.TextureHeight);

            // Draw Planets
            foreach (Planet planet in mPlanetSystem.mPlanetList)
            {
                planet.Draw();
            }

            mSpriteBatch.End();
        }

        public override void OnResolutionChanged()
        {
            // Uptdate Root to Window size
            mRoot.Update(new Rectangle(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height));
            mRootList.Update(new Rectangle(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height));
            mBackground.Update(new Rectangle(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height));

        }

        public void Exit()
        {
            // Exit Layer
            mLayerManager.PopLayer();
        }

        public override void Destroy() { }
    }
}
