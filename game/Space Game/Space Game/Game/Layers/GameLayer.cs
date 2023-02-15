using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.LayerManagement;
using rache_der_reti.Core.SoundManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Game.GameObjects;
using System;
using System.Runtime.ExceptionServices;

namespace Space_Game.Game.Layers
{
    [Serializable]
    public class GameLayer : Layer
    {
        [JsonProperty] private Camera2d mCamera2d;
        private PlanetSystem mPlanetSystem; 

        public GameLayer(LayerManager layerManager, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch,
            ContentManager contentManager, SoundManager soundManager)
        : base(layerManager, graphicsDevice, spriteBatch, contentManager, soundManager)
        {
            mSpriteBatch = spriteBatch;
            mCamera2d = new(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            mPlanetSystem = new PlanetSystem(mLayerManager,  mGraphicsDevice, mSpriteBatch, mContentManager, mSoundManager);
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            mCamera2d.Update(gameTime, inputState);
            mPlanetSystem.Update(gameTime , inputState, mCamera2d);
        }
        public override void Draw()
        {
            mSpriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mCamera2d.GetViewTransformationMatrix(), samplerState: SamplerState.PointClamp);
            mPlanetSystem.Draw();
            mSpriteBatch.End();
        }

        public override void Destroy()
        {

        }

        public override void OnResolutionChanged()
        {
            mCamera2d.SetResolution(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
        }

    }
}
