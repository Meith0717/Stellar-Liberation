﻿// SpaceShip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.AI;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.ProjectileManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.PropulsionSystem;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.WeaponSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.Recources.Items;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Enemys;
using System;
using System.Linq;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips
{
    public enum Fractions { Enemys, Allies }

    [Serializable]
    [Collidable]
    public abstract class SpaceShip : GameObject2D
    {
        [JsonIgnore] protected UtilityAi mAi;
        [JsonProperty] protected bool IsDestroyed { get; private set; }
        [JsonIgnore] public SensorSystem SensorArray { get; private set; }
        [JsonIgnore] public SublightDrive SublightEngine { get; private set; }
        [JsonIgnore] public HyperDrive HyperDrive { get; private set; }
        [JsonIgnore] public TurretSystem WeaponSystem { get; private set; }
        [JsonIgnore] public DefenseSystem DefenseSystem { get; private set; }
        [JsonIgnore] public Fractions Fraction;


        public SpaceShip(Vector2 position, string textureId, float textureScale, SensorSystem sensorArray, SublightDrive sublightEngine, TurretSystem weaponSystem, DefenseSystem defenseSystem, Fractions fractions)
            : base(position, textureId, textureScale, 10)
        {
            SensorArray = sensorArray;
            SublightEngine = sublightEngine;
            HyperDrive = new(500);
            WeaponSystem = weaponSystem;
            DefenseSystem = defenseSystem;
            Fraction = fractions;

        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {

            HyperDrive.Update(gameTime, this, scene);
            if (!HyperDrive.IsActive) SublightEngine.Update(gameTime, this);

            MovingDirection = Geometry.CalculateDirectionVector(Rotation);
            ContinuousCollisionDetection.ManageColisions(gameTime, this, scene.SpatialHashing);
            GameObject2DMover.Move(gameTime, this, scene);
            base.Update(gameTime, inputState, scene);

            if (DefenseSystem.HullPercentage <= 0 && !IsDestroyed) Explode(scene);

            HasProjectileHit(gameTime, scene);
            DefenseSystem.Update(gameTime);
            SensorArray.Scan(Position, Fraction, scene);
            if (!IsDestroyed) WeaponSystem.Update(gameTime, this, scene.GameLayer.CurrentSystem.GameObjects);
            mAi.Update(gameTime, this, scene);

            if (!IsDestroyed) return;
            Dispose = true;
        }

        private void HasProjectileHit(GameTime gameTime, Scene scene)
        {
            var projectileInRange = scene.SpatialHashing.GetObjectsInRadius<Projectile>(Position, (int)BoundedBox.Radius * 10);
            if (!projectileInRange.Any()) return;
            var hit = false;
            foreach (var projectile in projectileInRange)
            {
                if (projectile.Origine is EnemyShip && this is EnemyShip) continue;
                if (projectile.Origine == this) return;
                if (!ContinuousCollisionDetection.HasCollide(gameTime, projectile, this, out var position)) continue;

                projectile.HasCollide((Vector2)position, scene);
                DefenseSystem.GotHit((Vector2)position, projectile.ShieldDamage, projectile.HullDamage, scene);
                hit = true;
            }
            if (!hit) return;
            SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.torpedoHit);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);

            if (IsDestroyed) return;
            scene.GameLayer.DebugSystem.DrawSensorRadius(Position, SensorArray.ShortRangeScanDistance, scene);
            TextureManager.Instance.DrawGameObject(this);
            WeaponSystem.Draw(scene);
            SublightEngine.Draw(scene.GameLayer.DebugSystem, this, scene);
            DefenseSystem.DrawShields(this);
        }

        public void Explode(Scene scene)
        {
            IsDestroyed = true;
            Velocity = 0;

            var shakeamount = (500000 - Vector2.Distance(scene.Camera2D.Position, Position)) / 500000;
            if (shakeamount < 0) shakeamount = 0;
            scene.Camera2D.Shake((int)(shakeamount * 100));
            ExplosionEffect.ShipDestroyed(Position, scene.ParticleManager);

            scene.GameLayer.CurrentSystem.GameObjects.AddObj(ItemFactory.Get(ItemID.Iron, MovingDirection, Position));
            scene.GameLayer.CurrentSystem.GameObjects.AddObj(ItemFactory.Get(ItemID.Gold, MovingDirection, Position));
            scene.GameLayer.CurrentSystem.GameObjects.AddObj(ItemFactory.Get(ItemID.Titanium, MovingDirection, Position));
            scene.GameLayer.CurrentSystem.GameObjects.AddObj(ItemFactory.Get(ItemID.Platin, MovingDirection, Position));
            scene.GameLayer.CurrentSystem.GameObjects.AddObj(ItemFactory.Get(ItemID.QuantumCrystals, MovingDirection, Position));
            scene.GameLayer.CurrentSystem.GameObjects.AddObj(ItemFactory.Get(ItemID.DarkMatter, MovingDirection, Position));

            return;
        }
    }
}