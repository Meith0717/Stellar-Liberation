using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.Game.GameObjects.Weapons;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game
{
    [Serializable]
    public class GameState
    {
        // Saved Classes
        [JsonProperty] public float GameTime;
        [JsonProperty] public double mAlloys;
        [JsonProperty] public double mEnergy;
        [JsonProperty] public double mCrystals;
        [JsonProperty] public Player mMainShip;

        public List<Pirate> mPirates = new();
        private readonly WeaponManager mWeaponManager = new(); 

        // Unsaved Classes
        [JsonIgnore] private readonly GameEngine.GameEngine mGameEngine;
        [JsonIgnore] private readonly ParllaxManager mParllaxManager;

        public GameState(GameEngine.GameEngine gameEngine)
        {
            mGameEngine = gameEngine;
            mMainShip = new();

            mPirates.Add(new());

            mParllaxManager = new();
            mParllaxManager.Add(new("gameBackground", 0.05f));
            mParllaxManager.Add(new("gameBackgroundParlax1", 0.1f));
            mParllaxManager.Add(new("gameBackgroundParlax2", 0.15f));
            mParllaxManager.Add(new("gameBackgroundParlax3", 0.2f));
            mParllaxManager.Add(new("gameBackgroundParlax4", 0.25f));
        }

        public void Update(InputState inputState, GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            mMainShip.Update(gameTime, inputState, mGameEngine, mWeaponManager);
            foreach (var pirate in mPirates)
            {
                pirate.Update(gameTime, inputState, mGameEngine, mWeaponManager);  
            }
            mWeaponManager.Update(gameTime, inputState, mGameEngine);
            mGameEngine.UpdateEngine(gameTime, inputState, graphicsDevice);
            mParllaxManager.Update(mGameEngine.Camera.Movement, mGameEngine.Camera.Zoom);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            spriteBatch.End();

            mGameEngine.BeginWorldDrawing(spriteBatch);
            mGameEngine.RenderWorldObjectsOnScreen();
            mGameEngine.EndWorldDrawing(spriteBatch);
        }

        public void ApplyResolution(GraphicsDevice graphicsDevice)
        {
            mParllaxManager.OnResolutionChanged(graphicsDevice);
        }
    }
}
