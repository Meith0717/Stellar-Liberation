// CollisionPredictor.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;

namespace StellarLiberation.Game.Core.GameProceses.CollisionDetection
{
    public class CollisionPredictor
    {
        public static Vector2? PredictPosition(GameTime gameTime, Vector2 position, float speed, GameObject2D target)
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
