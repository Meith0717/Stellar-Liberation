// ContinuousCollisionDetection.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;

namespace StellarLiberation.Game.Core.GameProceses.CollisionDetection
{
    public static class ContinuousCollisionDetection
    {
        /// <summary>
        /// Checks for collision between two 2D game objects based on their bounding boxes, considering their current positions and predicted positions based on velocity.
        /// </summary>
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
                if (predictBoundBox.Intersects(collidingObj.BoundedBox)) return false;
            }
            collidingPosition = predictBoundBox.Position;
            return true;
        }

        /// <summary>
        /// Determines if one 2D game object is completely inside another by checking if the bounding box of the second object contains the position of the first object.
        /// </summary>
        public static bool IsInside(GameObject2D checkingObj, GameObject2D collidingObj)
        {
            var distance = Vector2.Distance(checkingObj.Position, collidingObj.Position);
            return distance < (checkingObj.BoundedBox.Radius + collidingObj.BoundedBox.Radius);
        }
    }
}
