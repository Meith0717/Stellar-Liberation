using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.Effects;
using Galaxy_Explovive.Core.GameEngine;
using Galaxy_Explovive.Core.GameEngine.InputManagement;
using Galaxy_Explovive.Core.GameEngine.Rendering;
using Galaxy_Explovive.Core.GameEngine.Utility;
using Galaxy_Explovive.Core.Map;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Galaxy_Explovive.Game
{
    [Serializable]
    public class GameState
    {
        // Saved Classes
        [JsonProperty] public float GameTime;
        [JsonProperty] public double mAlloys;
        [JsonProperty] public double mEnergy;
        [JsonProperty] public double mCrystals;
        [JsonProperty] private List<Cargo> mShips = new(); // Has to be set to JsonProperty !!!!
        [JsonProperty] public readonly Map mMap;

        // Unsaved Classes
        [JsonIgnore] private readonly GameEngine mGameEngine;
        [JsonIgnore] private readonly ParllaxManager mParllaxManager;

        public GameState(GameEngine gameEngine)
        {
            mGameEngine = gameEngine;
            mParllaxManager = new();
            mMap = new(3000000, 62500);
            mMap.Generate();
            mParllaxManager.Add(new("gameBackgroundParlax1", 0.1f));
            mParllaxManager.Add(new("gameBackgroundParlax2", 0.15f));
            mParllaxManager.Add(new("gameBackgroundParlax3", 0.2f));
            mParllaxManager.Add(new("gameBackgroundParlax4", 0.25f));
            mShips.Add(new(MyUtility.GetRandomVector2(Vector2.Zero, 0)));
        }

        public void Update(InputState inputState, GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            mGameEngine.UpdateEngine(gameTime, inputState, graphicsDevice);
            mGameEngine.UpdateGameObjects(gameTime, inputState, mShips);
            mMap.Update(gameTime, inputState, mGameEngine);

            if (inputState.mActionList.Contains(ActionType.ToggleRayTracing))
            {
                Globals.mRayTracing = !Globals.mRayTracing;
            }

            mParllaxManager.Update(mGameEngine.Camera.Movement, mGameEngine.Camera.Zoom);
        }

        public void Draw(SpriteBatch spriteBatch, TextureManager textureManager)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw(textureManager);
            spriteBatch.End();

            mGameEngine.BeginWorldDrawing(spriteBatch, textureManager);
            mGameEngine.DrawObjectsOnScreen(textureManager);
            mGameEngine.EndWorldDrawing(spriteBatch);
        }

        public void ApplyResolution(GraphicsDevice graphicsDevice)
        {
            mParllaxManager.OnResolutionChanged(graphicsDevice);
        }
    }
}
