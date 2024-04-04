// Asteroid.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Penumbra;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.Layers;
using System.Linq;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    public class Asteroid : GameObject2D, ICollidable
    {
        public float Mass { get => 50; }
        private readonly Hull mHull;

        public Asteroid(Vector2 position, string textureID, float textureScale)
            : base(position, textureID, textureScale, 50)
        {
            mHull = new(BoundedBox.GetPolygone());
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            base.Update(gameTime, gameState, planetsystemState);
            Rotation -= (float)(0.00001 * gameTime.ElapsedGameTime.TotalMilliseconds);
            Velocity = MathHelper.Clamp(Velocity - 0.001f, 0, float.PositiveInfinity);
            CheckForHit(gameTime, gameState, planetsystemState);
            Physics.HandleCollision(gameTime, this, planetsystemState.SpatialHashing);
            GameObject2DMover.Move(gameTime, this, planetsystemState.SpatialHashing);
            mHull.Position = Position;
        }

        private void CheckForHit(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            var projectileInRange = planetsystemState.SpatialHashing.GetObjectsInRadius<LaserProjectile>(Position, (int)BoundedBox.Diameter);
            if (!projectileInRange.Any()) return;
            var gotHit = false;
            Vector2? position = null;
            foreach (var projectile in projectileInRange)
            {
                if (!ContinuousCollisionDetection.HasCollide(gameTime, projectile, this, out position)) continue;
                projectile.HasCollide(Position, null);
                gotHit = true;
            }
            if (!gotHit) return;
            UpdateScale(TextureScale - 0.05f);
            if (TextureScale < 0.2f) IsDisposed = true;
            if (position is null) return;
            var momentum = Vector2.Normalize((Vector2)position - Position);
            ExplosionEffect.AsteroidHit((Vector2)position, momentum, planetsystemState.ParticleEmitors, gameState.GameSettings.ParticlesMultiplier);
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
