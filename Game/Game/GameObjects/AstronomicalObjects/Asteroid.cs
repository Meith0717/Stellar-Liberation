// Asteroid.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.ProjectileManagement;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using System.Linq;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    [Collidable(50)]
    public class Asteroid : GameObject2D
    {
        public Asteroid(Vector2 position, string textureID, float textureScale)
            : base(position, textureID, textureScale, 50) { }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            base.Update(gameTime, inputState, scene);
            Rotation += .01f;
            Velocity = MathHelper.Clamp(Velocity - 0.001f, 0, float.PositiveInfinity);
            CheckForHit(gameTime, scene);
            Physics.HandleCollision(gameTime, this, scene.SpatialHashing);
            GameObject2DMover.Move(gameTime, this, scene);
        }

        private void CheckForHit(GameTime gameTime, Scene scene)
        {
            var projectileInRange = scene.SpatialHashing.GetObjectsInRadius<Projectile>(Position, (int)BoundedBox.Diameter);
            if (!projectileInRange.Any()) return;
            var gotHit = false;
            Vector2? position = null;
            foreach (var projectile in projectileInRange)
            {
                if (!ContinuousCollisionDetection.HasCollide(gameTime, projectile, this, out position)) continue;
                projectile.HasCollide(Position, scene);
                gotHit = true;
            }
            if (!gotHit) return;
            TextureScale -= 0.05f;
            if (TextureScale < 0.2f) Dispose = true;
            if (position is null) return;
            var momentum = Vector2.Normalize((Vector2)position - Position);
            ExplosionEffect.AsteroidHit((Vector2)position, momentum, scene.ParticleManager);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
