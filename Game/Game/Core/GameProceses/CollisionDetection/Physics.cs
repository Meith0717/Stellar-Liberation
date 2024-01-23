// Physics.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Allies;
using System;

namespace StellarLiberation.Game.Core.GameProceses.CollisionDetection
{
    public class Physics
    {
        private static float MomentumConservation(float mass1, float mass2, float velocity2) 
            => float.IsInfinity(mass2) ? 0 : mass2 * velocity2 / mass1;

        private static bool TryGetGetMass(GameObject2D gameObject2D, out float mass)
        {
            mass = 0;
            var type = gameObject2D.GetType();
            var collisionAttribute = (CollidableAttribute)Attribute.GetCustomAttribute(type, typeof(CollidableAttribute));
            if (collisionAttribute == null) return false;
            mass = collisionAttribute.Mass;
            return true;
        }

        public static void HandleCollision(GameTime gameTime, GameObject2D gameObject2D, SpatialHashing<GameObject2D> spatialHashing)
        {
            if (!TryGetGetMass(gameObject2D, out var m1)) return;
            var objts = spatialHashing.GetObjectsInRadius<GameObject2D>(gameObject2D.Position, gameObject2D.BoundedBox.Diameter);
            objts.Remove(gameObject2D);
            foreach (var obj in objts)
            {
                if (!TryGetGetMass(obj, out var m2)) return;
                if (!ContinuousCollisionDetection.HasCollide(gameTime, gameObject2D, obj, out var _)) return;
                var v2 = obj.Velocity;
                var pushDir = - Vector2.Normalize(obj.Position - gameObject2D.Position);

                if (obj is Player)
                    System.Diagnostics.Debug.WriteLine("");

                gameObject2D.Velocity = MomentumConservation(m1, m2, v2);
                gameObject2D.MovingDirection += pushDir.IsNaN() ? Vector2.UnitX : pushDir;
            }
        }
    }
}
