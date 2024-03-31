// GameObject2DManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    public static class GameObject2DManager
    {
        public static void Initialize(SpatialHashing spatialHashing, ref GameObject2DList gameObject2DList)
        {
            foreach (var obj in gameObject2DList)
                spatialHashing.InsertObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
        }

        public static void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, ref GameObject2DList gameObject2DList)
        {
            var copyList = new List<GameObject2D>(gameObject2DList.ToList());
            foreach (var obj in copyList)
            {
                obj.Update(gameTime, inputState, gameLayer);

                if (!obj.IsDisposed) continue;
                if (!gameObject2DList.Remove(obj)) return;
                gameLayer.SpatialHashing.RemoveObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
            }
        }

    }
}
