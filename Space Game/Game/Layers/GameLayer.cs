using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.LayerManagement;

namespace Galaxy_Explovive.Game.Layers
{
    public class GameLayer : Layer
    {

        public GameState mGame;

        // Layer Stuff _____________________________________
        public GameLayer(Game1 game)
            : base(game)
        {
            mGame = new(mGraphicsDevice, mTextureManager, mSoundManager);
            GameState loadedGame = (GameState)mSerialize.PopulateObject(mGame, "save");
            if (loadedGame != null) { mGame = loadedGame; return; }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mGame.Update(inputState, gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mGame.Draw(spriteBatch);
        }

        public override void OnResolutionChanged()
        {
            mGame.ApplyResolution();
        }

        public override void Destroy() { }
    }
}
