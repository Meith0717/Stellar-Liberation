// CollisionPredictor.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.Core.GameProceses.CollisionDetection
{

    public enum MovementState
    {
        Expanding,
        Converging,
        MovingPerpendicular,
        None
    }

    public class CollisionPredictor
    {
        public static MovementState GetMovementState(Vector2 position, Vector2 targetPosition, Vector2 targetMovingDirection)
        {
            var dirToTarget = position.DirectionToVector2(targetPosition);
            var dotProduct = Vector2.Dot(dirToTarget, targetMovingDirection);
            return dotProduct switch
            {
                < 0 => MovementState.Converging,
                > 0 => MovementState.Expanding,
                0 => MovementState.MovingPerpendicular,
                _ => MovementState.None
            };
        }

        // TODO
        public static Vector2? PredictPosition(GameTime gameTime, Vector2 position, float speed, GameObject target)
        {
            if (target is null) return null;

            // Calculate the relative position of the target with respect to the shooter
            var distance = Vector2.Distance(position, target.Position);

            // Calculate the time of intersection using the relative motion equation
            float timeToIntersection = distance / (speed * gameTime.ElapsedGameTime.Milliseconds);

            // Calculate the future position of the target based on its velocity
            var pos = ExtendetRandom.NextVectorInCircle(target.BoundedBox);
            Vector2 futureTargetPosition = pos + target.Velocity * gameTime.ElapsedGameTime.Milliseconds * target.MovingDirection * timeToIntersection;

            return futureTargetPosition;
        }
    }
}
