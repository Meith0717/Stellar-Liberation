// GameObject2DManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    [Serializable]
    public static class GameObject2DManager
    {

        public static void SetSpatialHashing(SpatialHashing spatialHashing, ref GameObject2DTypeList gameObject2Ds)
        {
            foreach (var obj in gameObject2Ds) 
                spatialHashing.InsertObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
        }

        public static void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, ref readonly GameObject2DTypeList gameObject2Ds)
        {
            var copyList = new List<GameObject2D>(gameObject2Ds.ToList());
            foreach (var obj in copyList)
            {
                obj.Update(gameTime, inputState, gameLayer);

                if (!obj.IsDisposed) continue;
                if (!gameObject2Ds.Remove(obj)) return;
                gameLayer.SpatialHashing.RemoveObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
            }
        }

        public static void Update<T>(GameTime gameTime, InputState inputState, GameLayer gameLayer, ref readonly List<T> gameObject2Ds) where T : GameObject2D
        {
            var copyList = new List<GameObject2D>(gameObject2Ds);
            foreach (var obj in copyList)
            {
                obj.Update(gameTime, inputState, gameLayer);

                if (!obj.IsDisposed) continue;
                if (!gameObject2Ds.Remove((T)obj)) return;
                gameLayer.SpatialHashing.RemoveObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
            }
        }
    }
}
