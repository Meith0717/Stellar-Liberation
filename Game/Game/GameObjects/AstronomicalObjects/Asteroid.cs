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
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using StellarLiberation.Game.Core.Utilitys;
using System.Linq;

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
            foreach (var projectile in projectileInRange)
            {
                if (!ContinuousCollisionDetection.HasCollide(gameTime, projectile, this, out var _)) continue;
                projectile.HasCollide();
                gotHit = true;
            }
            if (gotHit) SoundManager.Instance.PlaySound("torpedoHit", ExtendetRandom.Random.Next(5, 8) / 10f);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
