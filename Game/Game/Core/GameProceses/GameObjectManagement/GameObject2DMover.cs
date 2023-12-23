// GameObject2DMover.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    public static class GameObject2DMover
    {
        public static void Move(GameTime gameTime, GameObject2D gameObject2D, Scene scene)
        {
            gameObject2D.RemoveFromSpatialHashing(scene);
            gameObject2D.Position += gameObject2D.MovingDirection * gameObject2D.Velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            gameObject2D.AddToSpatialHashing(scene);
        }
    }
}
