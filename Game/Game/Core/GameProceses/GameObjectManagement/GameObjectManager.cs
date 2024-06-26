﻿// GameObjectManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Layers;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    public static class GameObjectManager
    {
        public static void Initialize(SpatialHashing spatialHashing, ref GameObjectList gameObject2DList)
        {
            foreach (var obj in gameObject2DList.ListCopy())
                spatialHashing.InsertObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
        }

        public static void Initialize<T>(SpatialHashing spatialHashing, ref List<T> gameObject2DList) where T : GameObject
        {
            foreach (var obj in gameObject2DList)
                spatialHashing.InsertObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
        }

        public static void Initialize<T>(SpatialHashing spatialHashing, ref T obj) where T : GameObject => spatialHashing.InsertObject(obj, (int)obj.Position.X, (int)obj.Position.Y);

        public static void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState, ref GameObjectList gameObject2DList)
        {
            var copyList = gameObject2DList.ListCopy();
            foreach (var obj in copyList)
            {
                obj.Update(gameTime, gameState, planetsystemState);
                if (!obj.IsDisposed) continue;
                if (!gameObject2DList.Remove(obj)) return;
                planetsystemState.SpatialHashing.RemoveObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
            }
        }

        public static void Update<T>(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState, ref List<T> gameObject2DList) where T : GameObject
        {
            var copyList = new List<GameObject>(gameObject2DList.ToList());
            foreach (T obj in copyList)
            {
                obj.Update(gameTime, gameState, planetsystemState);
                if (!obj.IsDisposed) continue;
                if (!gameObject2DList.Remove(obj)) return;
                planetsystemState.SpatialHashing.RemoveObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
            }
        }

        public static void Update<T>(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState, ref T obj) where T : GameObject => obj.Update(gameTime, gameState, planetsystemState);
    }
}
