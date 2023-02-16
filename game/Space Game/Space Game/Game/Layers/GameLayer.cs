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
        [JsonProperty] private Camera2d mCamera2d;
        private List<PlanetSystem> mPlanetSystemList; 

        public GameLayer() : base()
        {
            mCamera2d = new(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            mCamera2d.SetPosition(Vector2.Zero);
            mPlanetSystemList = new List<PlanetSystem>();

            double probability = 1;
            int startAmount = 10000;
            double radius = 1000;
            float psi = 0f;

            while (startAmount > 0)
            {
                if (Globals.mRandom.NextDouble() <= probability)
                {
                    if (Globals.mRandom.NextDouble() <= 0.2) 
                    {
                        float newX = ((float)radius + Globals.mRandom.Next(-25, 25)) * MathF.Cos(psi);
                        float newY = ((float)radius + Globals.mRandom.Next(-25, 25)) * MathF.Sin(psi);
                        Vector2 newPosition = new Vector2(newX, newY);
                        mPlanetSystemList.Add(new PlanetSystem(newPosition));
                    }
                }
                radius += 0.5;
                psi += 0.05f;
                startAmount--;
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mCamera2d.Update(gameTime, inputState);
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Update(gameTime , inputState, mCamera2d);
            }
        }
        public override void Draw()
        {
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
