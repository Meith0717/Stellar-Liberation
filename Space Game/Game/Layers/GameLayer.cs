using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using rache_der_reti.Core.InputManagement;
using rache_der_reti.Core.LayerManagement;
using rache_der_reti.Core.Menu;
using rache_der_reti.Game.Layers;
using Space_Game.Core;
using Space_Game.Game.GameObjects;
using System;
using System.Collections.Generic;

namespace Space_Game.Game.Layers
{
    [Serializable]
    public class GameLayer : Layer
    {
        [JsonIgnore] public HudLayer mHudLayer;

        [JsonProperty] private PlanetSystem mHomeSystem;
        [JsonProperty] public double mPassedSeconds;
        [JsonProperty] private List<PlanetSystem> mPlanetSystemList = new();
        [JsonProperty] private List<Ship> mShipList = new();

        // Recources
        [JsonProperty] public double mAlloys;
        [JsonProperty] public double mEnergy;
        [JsonProperty] public double mCrystals;
        
        private UiElementSprite mBackground;
        

        public GameLayer() : base()
        {
            mBackground = new UiElementSprite("gameBackground");
            mBackground.mSpriteFit = UiElementSprite.SpriteFit.Cover;

            mHudLayer = new HudLayer();
            mPlanetSystemList = new List<PlanetSystem>();
            Globals.mCamera2d = new(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            Globals.mGameLayer = this;

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

            int random = Globals.mRandom.Next(mPlanetSystemList.Count);
            mHomeSystem = mPlanetSystemList[random];
            Globals.mCamera2d.mTargetPosition = mHomeSystem.Position;
            mShipList.Add(new Ship(mHomeSystem.Position + 
                new Vector2(Globals.mRandom.Next(-500, 500), Globals.mRandom.Next(-500, 500))));
            OnResolutionChanged();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mPassedSeconds += gameTime.ElapsedGameTime.Milliseconds / 1000d;
            Globals.mCamera2d.Update(gameTime, inputState);
            mHudLayer.Update(gameTime, inputState);
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Update(gameTime, inputState);
            }
            foreach (Ship ship in mShipList)
            {
                ship.Update(gameTime, inputState);
            }
        }
        public override void Draw()
        {
            mSpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mBackground.Render();
            mSpriteBatch.End();

            mSpriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: Globals.mCamera2d.GetViewTransformationMatrix(), samplerState: SamplerState.PointClamp);
            //TextureManager.GetInstance().Draw("Galaxy", Vector2.Zero - new Vector2(5000 * galaxyScale / 2, 5000 * galaxyScale / 2), 5000 * galaxyScale, 5000 * galaxyScale);
            foreach (PlanetSystem planetSystem in mPlanetSystemList)
            {
                planetSystem.Draw();
            }
            foreach (Ship ship in mShipList)
            {
                ship.Draw();
            }
            mSpriteBatch.End();
            mHudLayer.Draw();
        }

        public override void Destroy() { }

        public override void OnResolutionChanged()
        {
            Globals.mCamera2d.SetResolution(mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height);
            mBackground.Update(new Rectangle(0, 0, mGraphicsDevice.Viewport.Width, mGraphicsDevice.Viewport.Height));
        }

    }
}
