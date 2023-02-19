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
        private UiElementSprite mBackground;

        public GameLayer() : base()
        {
            mBackground = new UiElementSprite("gameBackground");
            mBackground.mSpriteFit = UiElementSprite.SpriteFit.Cover;

            mCamera2d = new(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            mCamera2d.SetPosition(Vector2.Zero);
            Globals.mCamera2d = mCamera2d;
            mPlanetSystemList = new List<PlanetSystem>();

            var startRadius = 0;
            var radAmount = 55;
            var radSteps = 3000;
            var probability = 0.5;
            bool lastPlaced = false;

            for (int radius = startRadius + 0; radius <= startRadius + radAmount * radSteps; radius+= radSteps)
            {
                float scope = 2 * MathF.PI * radius;
                float distribution = scope / 4200 * 2;
                float steps = MathF.PI * 2 / distribution;
                for (float angle = 0; angle < (MathF.PI * 2) - steps; angle+= steps)
                {
                    if (Globals.mRandom.NextDouble() <= probability && !lastPlaced) 
                    {
                        float newX = (radius + Globals.mRandom.Next(-radSteps/2, radSteps/2)) * MathF.Cos(angle);
                        float newY = (radius + Globals.mRandom.Next(-radSteps / 2, radSteps / 2)) * MathF.Sin(angle);
                        Vector2 newPosition = new Vector2(newX, newY);
                        mPlanetSystemList.Add(new PlanetSystem(newPosition));
                        lastPlaced = true;
                    }
                    else
                    {
                        lastPlaced = false;
                    }
                } 
                if (radius > radSteps * 30) { probability -= 0.015; }
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mCamera2d.Update(gameTime, inputState);
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Update(gameTime, inputState);
            }
            mBackground.Update(new Rectangle(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height));
        }
        public override void Draw()
        {
            mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mBackground.Render();
            mSpriteBatch.End();

            mSpriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: mCamera2d.GetViewTransformationMatrix(), samplerState: SamplerState.PointClamp);
            var galaxyScale = 40;
            //TextureManager.GetInstance().Draw("Galaxy", Vector2.Zero - new Vector2(5000 * galaxyScale / 2, 5000 * galaxyScale / 2), 5000 * galaxyScale, 5000 * galaxyScale);
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
