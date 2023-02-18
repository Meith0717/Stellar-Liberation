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
using System.Reflection.Metadata;
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
            Globals.mCamera2d = mCamera2d;
            mPlanetSystemList = new List<PlanetSystem>();

            int range = 20;
            int distace = 5000;

            for (int x = -range * 2; x <= range * 2; x++)
            {
                for (int y = -range; y <= range; y++)
                {
                    float newX = x * distace + Globals.mRandom.Next(-distace/2, distace/2);
                    float newY = y * distace + Globals.mRandom.Next(-distace/2, distace/2);
                    Vector2 newPosition = new Vector2(newX, newY);
                    mPlanetSystemList.Add(new PlanetSystem(newPosition));
                }
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mCamera2d.Update(gameTime, inputState);
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Update(gameTime, inputState, mCamera2d);
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
