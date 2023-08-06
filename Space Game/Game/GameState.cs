﻿using CelestialOdyssey.Game.Core;
using CelestialOdyssey.Game.Core.Inventory;
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

        public GameState(GameEngine.GameEngine gameEngine)
        {
            mGameEngine = gameEngine;
            mMainShip = new();

            for (int i = 0; i < 20; i++)
            {
                List<ItemType> types = new() { ItemType.odyssyum, ItemType.postyum };
                ItemsManager.SpawnItem(Utility.GetRandomVector2(-5000, 5000, -5000, 5000),
                    Utility.GetRandomElement(types));
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
            mMainShip.Update(gameTime, inputState, mGameEngine);
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
