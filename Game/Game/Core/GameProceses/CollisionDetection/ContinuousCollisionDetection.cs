// ContinuousCollisionDetection.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Utilitys;
using System;

namespace StellarLiberation.Game.Core.GameProceses.CollisionDetection
{
    public static class ContinuousCollisionDetection
    {
        public static bool HasCollide(GameTime gameTime, GameObject2D checkingObj, GameObject2D collidingObj, out Vector2? collidingPosition)
        {
            collidingPosition = null;

            // Check if Collision actually happened with the bounding box
            if (checkingObj.BoundedBox.Intersects(collidingObj.BoundedBox))
            {
                collidingPosition = checkingObj.Position;
                return true;
            }

            var futurePosition = checkingObj.Position + (checkingObj.MovingDirection * (checkingObj.Velocity * gameTime.ElapsedGameTime.Milliseconds));
            var frameDistance = Vector2.Distance(checkingObj.Position, futurePosition);

            // Frame Movement Distance of Obj smaler than BoundBox Radius. No further check needet!
            if (frameDistance < checkingObj.BoundedBox.Radius) return false;

            // Chech if Colliding Object is out of Frame Distance
            if (frameDistance < Vector2.Distance(checkingObj.Position, collidingObj.Position)) return false;

            // Object in Frame Distance
            var steps = frameDistance / checkingObj.BoundedBox.Radius;
            var predictBoundBox = checkingObj.BoundedBox;

            for (float frameStep = 1; frameStep <= steps; frameStep += 1)
            {
                var step = frameDistance / steps * frameStep;
                predictBoundBox.Position = checkingObj.Position + checkingObj.MovingDirection * step;

                // Check if Collision happened at this step
                if (predictBoundBox.Intersects(collidingObj.BoundedBox))
                {
                    collidingPosition = predictBoundBox.Position;
                    return true;
                }
            }
            return false;
        }
    }
}
