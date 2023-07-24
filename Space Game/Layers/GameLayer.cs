using CelestialOdyssey.Core.GameEngine;
using CelestialOdyssey.Core.GameEngine.InputManagement;
using CelestialOdyssey.Core.LayerManagement;
using CelestialOdyssey.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CelestialOdyssey.Layers
{
    internal class GameLayer : Layer
    {
        private readonly GameState mGameState;
        private readonly HudLayer mHudLayer;
        private bool mInitialized = false;

        public GameLayer(Game1 app) : base(app)
        {
            // Initialize all instances for the main game
            GameEngine engine = new(mSoundManager, new(mTextureManager, mGraphicsDevice.Viewport.Width / 2, 50));
            mGameState = new(engine);
            mHudLayer = new(app, engine, mGameState);
            
            // Checks if a game was saved
            GameState loadedGame = (GameState)mSerialize.PopulateObject(mGameState, "save");
            if (loadedGame != null) { mGameState = loadedGame; }
        }

        public override void Destroy() { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mGameState.Draw(spriteBatch, mTextureManager);
        }

        public override void OnResolutionChanged()
        {
            mGameState.ApplyResolution(mGraphicsDevice);
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mGameState.Update(inputState, gameTime, mGraphicsDevice);

            if (inputState.mActionList.Contains(ActionType.ToggleHeadUpDisplay) || !mInitialized)
            {
                mInitialized = true;
                if (mLayerManager.ContainsLayer(mHudLayer))
                {
                    mLayerManager.PopLayer();
                    return;
                }
                mLayerManager.AddLayer(mHudLayer);
            };
        }
    }
}
