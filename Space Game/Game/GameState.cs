using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;

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

        // Unsaved Classes
        [JsonIgnore] private readonly GameEngine.GameEngine mGameEngine;
        [JsonIgnore] private readonly ParllaxManager mParllaxManager;

        public GameState(GameEngine.GameEngine gameEngine)
        {
            mGameEngine = gameEngine;
            mMainShip = new();
            mParllaxManager = new();
            mParllaxManager.Add(new("gameBackground", 0.05f));
            mParllaxManager.Add(new("gameBackgroundParlax1", 0.1f));
            mParllaxManager.Add(new("gameBackgroundParlax2", 0.15f));
            mParllaxManager.Add(new("gameBackgroundParlax3", 0.2f));
            mParllaxManager.Add(new("gameBackgroundParlax4", 0.25f));
        }

        public void Update(InputState inputState, GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            if (inputState.mActionList.Contains(ActionType.Test))
            {
                mGameEngine.Camera.MoveToTarget(Vector2.Zero);
            }

            mMainShip.Update(gameTime, inputState, mGameEngine);
            mGameEngine.UpdateEngine(gameTime, inputState, graphicsDevice);
            mParllaxManager.Update(mGameEngine.Camera.Movement, mGameEngine.Camera.Zoom);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            spriteBatch.End();

            mGameEngine.BeginWorldDrawing(spriteBatch);
            mMainShip.Draw(mGameEngine);
            mGameEngine.RenderWorldObjectsOnScreen();
            mGameEngine.EndWorldDrawing(spriteBatch);
        }

        public void ApplyResolution(GraphicsDevice graphicsDevice)
        {
            mParllaxManager.OnResolutionChanged(graphicsDevice);
        }
    }
}
