using CelestialOdyssey.Core;
using CelestialOdyssey.Core.Effects;
using CelestialOdyssey.Core.GameEngine;
using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Core.GameEngine.InputManagement;

using CelestialOdyssey.Core.GameEngine.Utility;
using CelestialOdyssey.Core.GameObject;
using CelestialOdyssey.Core.Map;
using CelestialOdyssey.Game.GameObjects;
using CelestialOdyssey.Game.GameObjects.Spacecraft.SpaceShips;
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
        [JsonProperty] private List<Species> mSpecies = new(); // Has to be set to JsonProperty !!!!
        [JsonProperty] public readonly Map mMap;

        // Unsaved Classes
        [JsonIgnore] private readonly GameEngine mGameEngine;
        [JsonIgnore] private readonly ParllaxManager mParllaxManager;
        [JsonIgnore] private readonly CrossHair mSelectObjCrossHair;

        public GameState(GameEngine gameEngine)
        {
            mGameEngine = gameEngine;
            mParllaxManager = new();
            mMap = new(9000000, 67600);
            mMap.Generate();
            mParllaxManager.Add(new("gameBackground", 0.05f));
            mParllaxManager.Add(new("gameBackgroundParlax1", 0.1f));
            mParllaxManager.Add(new("gameBackgroundParlax2", 0.15f));
            mParllaxManager.Add(new("gameBackgroundParlax3", 0.2f));
            mParllaxManager.Add(new("gameBackgroundParlax4", 0.25f));
            Species terran = new("Terran");
            terran.SpawnScience(Vector2.Zero);
            mSpecies.Add(terran);
            mSelectObjCrossHair = new(CrossHair.CrossHairType.Select);
        }

        public void Update(InputState inputState, GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            if (inputState.mActionList.Contains(ActionType.Test))
            {
                mGameEngine.Camera.MoveToTarget(Vector2.Zero);
            }

            mGameEngine.UpdateEngine(gameTime, inputState, graphicsDevice);
            foreach(Species species in mSpecies)
            {
                mGameEngine.UpdateGameObjects(gameTime, inputState, species.Ships);
            }
            mMap.Update(gameTime, inputState, mGameEngine);
            mParllaxManager.Update(mGameEngine.Camera.Movement, mGameEngine.Camera.Zoom);
            mSelectObjCrossHair.Update(null, 0, Color.Transparent, false);
            if (mGameEngine.SelectObject == null) return;
            mSelectObjCrossHair.Update(mGameEngine.SelectObject.Position, 
                mGameEngine.SelectObject.TextureScale * 20, Color.OrangeRed, false);
        }

        public void Draw(SpriteBatch spriteBatch, TextureManager textureManager)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw(textureManager);
            spriteBatch.End();

            mGameEngine.BeginWorldDrawing(spriteBatch, textureManager);
            mMap.Draw(textureManager, mGameEngine);
            mGameEngine.RenderWorldObjectsOnScreen(textureManager);
            mSelectObjCrossHair.Draw(textureManager, mGameEngine);
            mGameEngine.EndWorldDrawing(spriteBatch, textureManager);
        }

        public void ApplyResolution(GraphicsDevice graphicsDevice)
        {
            mParllaxManager.OnResolutionChanged(graphicsDevice);
        }
    }
}
