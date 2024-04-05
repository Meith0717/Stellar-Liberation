// GameObject2DMover.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    public static class GameObjectMover
    {
        public static void Move(GameTime gameTime, GameObject gameObject2D, SpatialHashing spatialHashing)
        {
            spatialHashing?.RemoveObject(gameObject2D, (int)gameObject2D.Position.X, (int)gameObject2D.Position.Y);
            gameObject2D.Position += gameObject2D.MovingDirection * gameObject2D.Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            gameObject2D.BoundedBox.Position = gameObject2D.Position;
            spatialHashing?.InsertObject(gameObject2D, (int)gameObject2D.Position.X, (int)gameObject2D.Position.Y);
        }
    }
}
