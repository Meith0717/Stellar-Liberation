// CollisionPredictor.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.Collision_Detection
{
    public class CollisionPredictor
    {
        public static Vector2? PredictPosition(GameTime gameTime, Vector2 position, float speed, MovingObject target)
        {
            if (target is null) return null;

            // Calculate the relative position of the target with respect to the shooter
            var distance = Vector2.Distance(position, target.Position);

            // Calculate the time of intersection using the relative motion equation
            float timeToIntersection = distance / (speed * gameTime.ElapsedGameTime.Milliseconds);

            // Calculate the future position of the target based on its velocity
            Vector2 futureTargetPosition = target.Position + (target.Velocity * gameTime.ElapsedGameTime.Milliseconds * target.Direction) * timeToIntersection;

            return futureTargetPosition;
        }
    }
}
