// ContinuousCollisionDetection.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjects;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.Collision_Detection
{
    public static class ContinuousCollisionDetection
    {
        public static bool HasCollide(MovingObject checkingObj, GameObject collidingObj, out Vector2 collidingPosition)
        {
            collidingPosition = Vector2.Zero;

            var step = 50;
            var frameDistance = Vector2.Distance(checkingObj.Position, checkingObj.FuturePosition);
            var checkingBoundBox = checkingObj.BoundedBox;

            for (float frameStep = step; frameStep < frameDistance; frameStep += step)
            {
                collidingPosition = checkingBoundBox.Position;
                if (checkingBoundBox.Intersects(collidingObj.BoundedBox)) return true;
                checkingBoundBox.Position = checkingObj.Position + (checkingObj.Direction * frameStep);
            }

            return false;
        }
    }
}
