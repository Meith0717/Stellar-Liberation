// ContinuousCollisionDetection.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.GameObjectManagement;
using Microsoft.Xna.Framework;

namespace StellarLiberation.Game.Core.Collision_Detection
{
    public static class ContinuousCollisionDetection
    {
        public static bool HasCollide(MovingObject checkingObj, GameObject collidingObj, out Vector2 collidingPosition)
        {
            collidingPosition = Vector2.Zero;

            var step = 25;
            var frameDistance = Vector2.Distance(checkingObj.Position, checkingObj.FuturePosition);
            var predictBoundBox = checkingObj.BoundedBox;

            if (predictBoundBox.Intersects(collidingObj.BoundedBox)) return true;

            for (float frameStep = step; frameStep < frameDistance; frameStep += step)
            {
                predictBoundBox.Position = checkingObj.Position + (checkingObj.Direction * frameStep);
                collidingPosition = predictBoundBox.Position;
                if (predictBoundBox.Intersects(collidingObj.BoundedBox)) return true;
            }

            return false;
        }
    }
}
