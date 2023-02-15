using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Timers;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.LayerManagement;
using rache_der_reti.Core.Menu;
using rache_der_reti.Core.SoundManagement;
using rache_der_reti.Core.TextureManagement;
using Space_Game.Core;
using Space_Game.Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;

namespace Space_Game.Game.Layers
{
    [Serializable]
    public class GameLayer : Layer
    {
        private UiElementSprite mBackground;
        [JsonProperty] private Camera2d mCamera2d;
        private List<PlanetSystem> mPlanetSystemList; 

        public GameLayer() : base()
        {
            mBackground = new UiElementSprite("nebula");
            mBackground.BackgroundColor = Color.Black;
            mBackground.BackgroundAlpha = 0.65f;
            mBackground.mSpriteFit = UiElementSprite.SpriteFit.Cover;
            mCamera2d = new(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            mPlanetSystemList = new List<PlanetSystem>();
            for (int x = -50; x <= 50; x++)
            {
                for (int y = -50; y <= 50; y++)
                {
                    if (Globals.mRandom.NextDouble() < 0.1d)
                    {
                        var newX = x * 50 + Globals.mRandom.Next(-40, 40);
                        var newY = y * 50 + Globals.mRandom.Next(-40, 40);

                        mPlanetSystemList.Add(new PlanetSystem(new Vector2(newX, newY)));
                    }
                }
            }
        }
        public override void Update(GameTime gameTime, InputState inputState)
        {
            mBackground.Update(new Rectangle(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height));
            mCamera2d.Update(gameTime, inputState);
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Update(gameTime , inputState, mCamera2d);
            }
        }
        public override void Draw()
        {
            mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mBackground.Render();
            mSpriteBatch.End();

            mSpriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mCamera2d.GetViewTransformationMatrix(), samplerState: SamplerState.PointClamp);
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Draw();
            }
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
