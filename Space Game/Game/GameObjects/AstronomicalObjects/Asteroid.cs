// Asteroid.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.Collision_Detection;
using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System.Linq;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    public class Asteroid : GameObject
    {
        public Asteroid(Vector2 position) 
            : base(position, ContentRegistry.asteroid1, 0.1f, 50)
        {

        }

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            Rotation += .01f;
            CheckForHit(scene);
        }

        private void CheckForHit(Scene scene)
        {
            var projectileInRange = scene.GetObjectsInRadius<Projectile>(Position, (int)BoundedBox.Radius);
            if (!projectileInRange.Any()) return;
            var gotHit = false;
            foreach (var projectile in projectileInRange)
            {
                if (!ContinuousCollisionDetection.HasCollide(projectile, this, out var _)) continue;
                projectile.HasCollide();
                gotHit = true;
            }
            if (gotHit) SoundManager.Instance.PlaySound("torpedoHit", Utility.Random.Next(5, 8) / 10f);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
