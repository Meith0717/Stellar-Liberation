using CelestialOdyssey.Game.Core.Inventory;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.GameEngine.Content_Management;
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
        [JsonProperty] public Player mMainShip;

        [JsonIgnore] public MapItemsManager ItemsManager = new();

        // Unsaved Classes
        [JsonIgnore] private readonly GameEngine.GameEngine mGameEngine;
        [JsonIgnore] private readonly ParllaxManager mParllaxManager;
        [JsonIgnore] private readonly List<Pirate> Test = new();
        [JsonIgnore] private readonly Map map = new();

        public GameState(GameEngine.GameEngine gameEngine)
        {
            mGameEngine = gameEngine;
            map.Generate(mGameEngine);
            mMainShip = new(new(map.Width / 2, map.Height / 2));
            
            for (int i = 0; i < 1000; i++)
            {
                Test.Add(new(Utility.GetRandomVector2(0, map.Width, 0, map.Height))); 
            }

            mParllaxManager = new();
            mParllaxManager.Add(new(ContentRegistry.gameBackground.Name, 0.05f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax.Name, 0.1f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax1.Name, 0.15f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax2.Name, 0.2f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax3.Name, 0.25f));
        }

        public void Update(InputState inputState, GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            mGameEngine.UpdateEngine(gameTime, inputState, graphicsDevice);
            map.Update(gameTime, inputState, mGameEngine);
            mMainShip.Update(gameTime, inputState, mGameEngine);
            List<Pirate> list = new List<Pirate>();
            foreach (var item in Test)
            {
                item.Update(gameTime, inputState, mGameEngine);
                if (item.mHullForce > 0) continue; 
                list.Add(item);
                item.RemoveFromSpatialHashing(mGameEngine);
            }
            foreach (var item in list) { Test.Remove(item); }
            ItemsManager.Update(gameTime, inputState, mGameEngine);
            mParllaxManager.Update(mGameEngine.Camera.Movement, mGameEngine.Camera.Zoom);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            spriteBatch.End();

            mGameEngine.BeginWorldDrawing(spriteBatch);
            map.DrawSectores(mGameEngine);
            mGameEngine.RenderWorldObjectsOnScreen();
            mGameEngine.EndWorldDrawing(spriteBatch);
        }

        public void ApplyResolution(GraphicsDevice graphicsDevice)
        {
            mParllaxManager.OnResolutionChanged(graphicsDevice);
        }
    }
}
