// SpaceShip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.AI;
using StellarLiberation.Game.Core.Collision_Detection;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects;
using System;
using System.Linq;

namespace StellarLiberation.Game.Core.SpaceShipManagement
{
    public enum Factions { Enemys, Allies }

    [Serializable]
    [Collidable]
    public abstract class SpaceShip : GameObject2D
    {
        [JsonIgnore] protected UtilityAi mAi;
        [JsonProperty] protected bool IsDestroyed { get; private set; }
        [JsonIgnore] public SensorArray SensorArray { get; private set; }
        [JsonIgnore] public SublightEngine SublightEngine { get; private set; }
        [JsonIgnore] public HyperDrive HyperDrive { get; private set; }
        [JsonIgnore] public TurretBattery WeaponSystem { get; private set; }
        [JsonProperty] public DefenseSystem DefenseSystem { get; private set; }
        [JsonIgnore] protected Factions mOpponent;


        public SpaceShip(Vector2 position, string textureId, float textureScale, SensorArray sensorArray, SublightEngine sublightEngine, TurretBattery weaponSystem, DefenseSystem defenseSystem, Factions opponent)
            : base(position, textureId, textureScale, 10)
        {
            SensorArray = sensorArray;
            SublightEngine = sublightEngine;
            HyperDrive = new(500, 500);
            WeaponSystem = weaponSystem;
            DefenseSystem = defenseSystem;
            mOpponent = opponent;

        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {

            HyperDrive.Update(gameTime, this);
            if (!HyperDrive.IsActive) SublightEngine.Update(gameTime, this);

            MovingDirection = Geometry.CalculateDirectionVector(Rotation);
            ContinuousCollisionDetection.ManageColisions(gameTime, this, scene.SpatialHashing);
            GameObject2DMover.Move(gameTime, this, scene);
            base.Update(gameTime, inputState, scene);

            if (DefenseSystem.HullPercentage <= 0 && !IsDestroyed) Explode(scene);

            HasProjectileHit(gameTime, scene);
            DefenseSystem.Update(gameTime);
            SensorArray.Update(gameTime, Position, scene.GameLayer.CurrentSystem, scene, mOpponent);
            if (!IsDestroyed) WeaponSystem.Update(gameTime, this, scene.GameLayer.ProjectileManager, SensorArray.AimingShip);
            mAi.Update(gameTime, this, scene);

            if (!IsDestroyed) return;
            Dispose = true;
        }

        private void HasProjectileHit(GameTime gameTime, Scene scene)
        {
            var projectileInRange = scene.SpatialHashing.GetObjectsInRadius<Projectile>(Position, (int)BoundedBox.Radius);
            if (!projectileInRange.Any()) return;
            var hit = false;
            foreach (var projectile in projectileInRange)
            {
                if (projectile.Origine is Enemy && this is Enemy) continue;
                if (projectile.Origine == this) return;
                if (!ContinuousCollisionDetection.HasCollide(gameTime, projectile, this, out var position)) continue;

                projectile.HasCollide();
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

            for (int i = 0; i < 5; i++) scene.GameLayer.ItemManager.PopItem(MovingDirection, Position, new Items.Metall());

            return;
        }
    }
}