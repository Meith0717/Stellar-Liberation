// Physics.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using System;

namespace StellarLiberation.Game.Core.GameProceses.CollisionDetection
{
    public class Physics
    {
        private static void MomentumConservation(float m1, float m2, float v1, float v2, out float v)
        {
            v = 0;
            if (float.IsInfinity(m1) || float.IsInfinity(m2)) return;
            if (v1 == 0 && v2 == 0) return;
            v = ((m1 * v1) + (m2 * v2)) / (m1 + m2);
        }

        private static float PushOutVelocity(Vector2 position, float maxVelocity, CircleF boundBox)
        {
            var distance = Vector2.Distance(boundBox.Position, position);
            return maxVelocity * ((1 + MathF.Cos(distance * MathF.PI / boundBox.Diameter)) / 2);
        }

        private static bool TryGetGetMass(GameObject gameObject2D, out float mass)
        {
            mass = 0;
            if (gameObject2D is not ICollidable) return false;
            var collidable = (ICollidable)gameObject2D;
            mass = collidable.Mass;
            return true;
        }

        public static void HandleCollision(GameTime gameTime, GameObject gameObject2D, SpatialHashing spatialHashing)
        {
            if (!TryGetGetMass(gameObject2D, out var m1)) return;
            var objts = spatialHashing.GetObjectsInRadius<GameObject>(gameObject2D.Position, gameObject2D.BoundedBox.Radius);

            var acceleration = Vector2.Zero;

            foreach (var obj in objts)
            {
                if (obj == gameObject2D) continue;
                if (!TryGetGetMass(obj, out var m2)) continue;
                if (!ContinuousCollisionDetection.HasCollide(gameTime, gameObject2D, obj, out var _)) continue;

                // Calc direction
                var pushDir = Vector2.Normalize(gameObject2D.Position - obj.Position);
                if (pushDir.IsNaN())
                    pushDir = Vector2.UnitX;

                MomentumConservation(m1, m2, gameObject2D.Velocity, obj.Velocity, out var v);
                acceleration += pushDir * v;
            }
            if (acceleration.X == 0 & acceleration.Y == 0) return;
            gameObject2D.MovingDirection += acceleration.NormalizedCopy();
            gameObject2D.Velocity *= acceleration.Length();
        }
    }
}
