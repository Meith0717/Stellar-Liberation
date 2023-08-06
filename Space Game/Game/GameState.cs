using CelestialOdyssey.Game.Core.Inventory;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.Core.WeaponSystem;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Timers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

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
        [JsonIgnore] private readonly WeaponManager mWeaponManager;
        [JsonIgnore] private readonly List<Pirate> Test = new();

        public GameState(GameEngine.GameEngine gameEngine)
        {
            mGameEngine = gameEngine;
            mMainShip = new();
            mWeaponManager = new();

            for (int i = 0; i < 10; i++)
            {
                List<ItemType> types = new() { ItemType.odyssyum, ItemType.postyum };
                ItemsManager.SpawnItem(Utility.GetRandomVector2(-5000, 5000, -5000, 5000),
                    Utility.GetRandomElement(types));
            }

            for (int i = 0; i < 10; i++)
            {
                Test.Add(new(Utility.GetRandomVector2(-10000, 10000, -10000, 10000))); 
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
            mMainShip.Update(gameTime, inputState, mGameEngine, mWeaponManager);
            List<Pirate> list = new List<Pirate>();
            foreach (var item in Test)
            {
                item.Update(gameTime, inputState, mGameEngine, mWeaponManager);
                if (item.mHullForce > 0) continue; 
                list.Add(item);
                item.RemoveFromSpatialHashing(mGameEngine);
            }
            foreach (var item in list) { Test.Remove(item); }
            mWeaponManager.Update(gameTime, inputState, mGameEngine);
            ItemsManager.Update(gameTime, inputState, mGameEngine);
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
