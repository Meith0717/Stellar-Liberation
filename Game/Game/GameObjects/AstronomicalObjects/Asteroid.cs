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
using StellarLiberation.Game.Layers;
using System.Linq;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects
{
    public class Asteroid : GameObject
    {
        public Asteroid(Vector2 position) 
            : base(position, TextureRegistries.asteroid1, 0.1f, 50) {; }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            base.Update(gameTime, inputState, scene);
            Rotation += .01f;
            CheckForHit(scene);
        }

        private void CheckForHit(Scene scene)
        {
            var projectileInRange = scene.SpatialHashing.GetObjectsInRadius<Projectile>(Position, (int)BoundedBox.Radius);
            if (!projectileInRange.Any()) return;
            var gotHit = false;
            foreach (var projectile in projectileInRange)
            {
                if (!ContinuousCollisionDetection.HasCollide(projectile, this, out var _)) continue;
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
