// GameObject2DMover.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    public static class GameObject2DMover
    {
        public static void Move(GameTime gameTime, GameObject2D gameObject2D, SpatialHashing<GameObject2D> spatialHashing)
        {
            spatialHashing?.RemoveObject(gameObject2D, (int)gameObject2D.Position.X, (int)gameObject2D.Position.Y);
            gameObject2D.Position += gameObject2D.MovingDirection * gameObject2D.Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            spatialHashing?.InsertObject(gameObject2D, (int)gameObject2D.Position.X, (int)gameObject2D.Position.Y);
        }
    }
}
