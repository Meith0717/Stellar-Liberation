// Asteroid.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.Collision_Detection;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using StellarLiberation.Game.Core.Utilitys;
using System.Linq;
using static StellarLiberation.Game.GameObjects.Items;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    [Collidable]
    public class Asteroid : GameObject2D
    {
        public Asteroid(Vector2 position) 
            : base(position, TextureRegistries.asteroid1, 5f, 50) {; }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            RemoveFromSpatialHashing(scene);
            base.Update(gameTime, inputState, scene);
            Rotation += .01f;
            CheckForHit(gameTime, scene);
            AddToSpatialHashing(scene);
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
                projectile.HasCollide();
                gotHit = true;
            }
            if (!gotHit) return;
            TextureScale -= 0.05f;
            if (TextureScale < 0.1f) Dispose = true;
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
