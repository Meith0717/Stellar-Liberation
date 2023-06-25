 using Galaxy_Explovive.Core.Debug;
using Galaxy_Explovive.Core.Effects;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.Map;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.UserInterface.Messages;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using Galaxy_Explovive.Game.GameObjects;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.Utility;
using Newtonsoft.Json;

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
            mGameEngine.UpdateGameObjects(gameTime, inputState, mMap.PlanetSystems);

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

            mGameEngine.BeginWorldDrawing(spriteBatch);
            mGameEngine.DrawGameObjects(textureManager, mMap.PlanetSystems);
            mGameEngine.DrawGameObjects(textureManager, mShips);
            spriteBatch.End();
        }

        public void ApplyResolution(GraphicsDevice graphicsDevice)
        {
            mParllaxManager.OnResolutionChanged(graphicsDevice);
        }
    }
}
