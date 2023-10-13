// SpaceShip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.Animations;
using CelestialOdyssey.Game.Core.Collision_Detection;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.ShipSystems;
using CelestialOdyssey.Game.Core.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips.Enemy;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using rache_der_reti.Core.Animation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public abstract class SpaceShip : MovingObject
    {
        [JsonProperty] public bool IsDestroyed { get; protected set; }
        [JsonIgnore] public SensorArray SensorArray { get; protected set; } = new(10000, 1000);
        [JsonIgnore] public SublightEngine SublightEngine { get; protected set; } = new(2.5f);
        [JsonIgnore] public HyperDrive HyperDrive { get; protected set; } = new(6000, 100);
        [JsonIgnore] public WeaponSystem WeaponSystem { get; protected set; }
        [JsonProperty] public DefenseSystem DefenseSystem { get; protected set; } = new(100, 100, 0, 1);
        [JsonIgnore] public PlanetSystem ActualPlanetSystem { get; set; }
        [JsonIgnore] protected SpriteSheet ExplosionSheet;


        public SpaceShip(Vector2 position, string textureId, float textureScale)
            : base(position, textureId, textureScale, 10)
        {
            List<int> f = Enumerable.Range(0, 8 * 8 - 1).ToList();
            f.Add(0);

            ExplosionSheet = new(ContentRegistry.explosion, 64, 3, TextureScale * 10);
            ExplosionSheet.Animate("destroy", new(60, Animation.GetRowList(1, 64), false));
        }


        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            Direction = Geometry.CalculateDirectionVector(Rotation);
            base.Update(gameTime, inputState, sceneManagerLayer, scene);

            ExplosionSheet.Update(gameTime, Position);
            SublightEngine.Update(this);

            if (DefenseSystem.HullLevel <= 0 && !IsDestroyed) Explode();
            if (IsDestroyed) return;

            HasProjectileHit(scene);
            HyperDrive.Update(gameTime, this);
            DefenseSystem.Update(gameTime);
            SensorArray.Update(gameTime, Position, ActualPlanetSystem, scene);
            WeaponSystem.Update(gameTime, inputState, this, sceneManagerLayer, scene);
        }

        private void HasProjectileHit(Scene scene)
        {
            var projectileInRange = scene.GetObjectsInRadius<Projectile>(Position, (int)BoundedBox.Radius);
            if (!projectileInRange.Any()) return;
            var gotDamage = false;
            foreach (var projectile in projectileInRange)
            {
                if (projectile.Origine is Enemy && this is Enemy) continue;
                if (projectile.Origine == this) return;
                if (!ContinuousCollisionDetection.HasCollide(projectile, this, out var _)) continue;

                projectile.HasCollide();
                DefenseSystem.GetDamage(projectile.ShieldDamage, projectile.HullDamage);
                gotDamage = true;
            }
            if (gotDamage) SoundManager.Instance.PlaySound("torpedoHit", Utility.Random.Next(5, 8) / 10f);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            ExplosionSheet.Draw(TextureDepth + 1);

            if (IsDestroyed) return;
            sceneManagerLayer.DebugSystem.DrawSensorRadius(Position, SensorArray.ScanRadius, scene);
            TextureManager.Instance.DrawGameObject(this);
            DefenseSystem.DrawShields(this);
            WeaponSystem.Draw(sceneManagerLayer, scene);
        }

        public void Explode()
        {
            ExplosionSheet.Play("destroy");
            IsDestroyed = true;
            SublightEngine.SetTarget(this, null);
            return;
        }
    }
}