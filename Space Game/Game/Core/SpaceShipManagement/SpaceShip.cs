// SpaceShip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.AI;
using CelestialOdyssey.Game.Core.Animations;
using CelestialOdyssey.Game.Core.Collision_Detection;
using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using rache_der_reti.Core.Animation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement
{
    [Serializable]
    public abstract class SpaceShip : MovingObject
    {
        [JsonIgnore] protected SpriteSheet ExplosionSheet;
        [JsonIgnore] protected BehaviorBasedAI mAi;
        [JsonProperty] protected bool IsDestroyed { get; private set; }
        [JsonIgnore] public SensorArray SensorArray { get; private set; }
        [JsonIgnore] public SublightEngine SublightEngine { get; private set; }
        [JsonIgnore] public HyperDrive HyperDrive { get; private set; }
        [JsonIgnore] public WeaponSystem WeaponSystem { get; private set; }
        [JsonProperty] public DefenseSystem DefenseSystem { get; private set; }
        [JsonIgnore] public PlanetSystem ActualPlanetSystem { get; set; }


        public SpaceShip(Vector2 position, string textureId, float textureScale, SensorArray sensorArray, SublightEngine sublightEngine, HyperDrive hyperDrive, WeaponSystem weaponSystem, DefenseSystem defenseSystem)
            : base(position, textureId, textureScale, 10)
        {
            this.SensorArray = sensorArray;
            this.SublightEngine = sublightEngine;
            this.HyperDrive = hyperDrive;
            this.WeaponSystem = weaponSystem;
            this.DefenseSystem = defenseSystem;

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

            HasProjectileHit(scene);
            HyperDrive.Update(gameTime, this);
            DefenseSystem.Update(gameTime);
            SensorArray.Update(gameTime, Position, ActualPlanetSystem, scene);
            WeaponSystem.Update(gameTime, inputState, this, sceneManagerLayer, scene, ActualPlanetSystem.ProjectileManager);
            mAi.Update(gameTime, SensorArray, this);

            if (!IsDestroyed) return;
            if (ExplosionSheet.IsActive("destroy")) return;
            Dispose = true;
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
            if (gotDamage) SoundManager.Instance.PlaySound("torpedoHit", Utility.Utility.Random.Next(5, 8) / 10f);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            ExplosionSheet.Draw(TextureDepth + 1);

            if (IsDestroyed) return;
            sceneManagerLayer.DebugSystem.DrawSensorRadius(Position, SensorArray.ScanRadius, scene);
            TextureManager.Instance.DrawGameObject(this);
            WeaponSystem.Draw(sceneManagerLayer, scene);
            DefenseSystem.DrawShields(this);
            DefenseSystem.DrawLive(this);
            SublightEngine.Draw(this, scene);
        }

        public void Explode()
        {
            ExplosionSheet.Play("destroy");
            IsDestroyed = true;
            SublightEngine.SetTarget(this, null);

            for (int i = 0; i < 5; i++) ActualPlanetSystem.ItemManager.PopItem(Direction, Position, new Items.Metall());

            return;
        }
    }
}