﻿// SpaceShip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using rache_der_reti.Core.Animation;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.AI;
using StellarLiberation.Game.Core.Animations;
using StellarLiberation.Game.Core.Collision_Detection;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
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
    public abstract class SpaceShip : MovingObject
    {
        [JsonIgnore] protected SpriteSheet ExplosionSheet;
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
            HyperDrive = new();
            WeaponSystem = weaponSystem;
            DefenseSystem = defenseSystem;
            mOpponent = opponent;

            ExplosionSheet = new(TextureRegistries.explosion, 64, 3, TextureScale * 10);
            ExplosionSheet.Animate("destroy", new(60, Animation.GetRowList(1, 64), false));
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            Direction = Geometry.CalculateDirectionVector(Rotation);
            base.Update(gameTime, inputState, scene);

            ExplosionSheet.Update(gameTime, Position);
            SublightEngine.Update(gameTime, this);

            if (DefenseSystem.HullLevel <= 0 && !IsDestroyed) Explode(scene);

            HasProjectileHit(scene);
            DefenseSystem.Update(gameTime);
            SensorArray.Update(gameTime, Position, scene.GameLayer.CurrentSystem, scene, mOpponent);
            if (!IsDestroyed) WeaponSystem.Update(gameTime, this, scene.GameLayer.ProjectileManager, SensorArray.AimingShip);
            mAi.Update(gameTime, SensorArray, this);

            if (!IsDestroyed) return;
            if (ExplosionSheet.IsActive("destroy")) return;
            Dispose = true;
        }

        private void HasProjectileHit(Scene scene)
        {
            var projectileInRange = scene.GetObjectsInRadius<Projectile>(Position, (int)BoundedBox.Radius);
            if (!projectileInRange.Any()) return;
            var hits = 0;
            foreach (var projectile in projectileInRange)
            {
                if (projectile.Origine is Enemy && this is Enemy) continue;
                if (projectile.Origine == this) return;
                if (!ContinuousCollisionDetection.HasCollide(projectile, this, out var _)) continue;

                projectile.HasCollide();
                DefenseSystem.GetDamage(projectile.ShieldDamage, projectile.HullDamage);
                hits++;
            }
            if (hits == 0) return;
            SoundManager.Instance.PlaySound("torpedoHit", ExtendetRandom.Random.Next(5, 8) / 10f);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            ExplosionSheet.Draw(TextureDepth + 1);

            if (IsDestroyed) return;
            scene.GameLayer.DebugSystem.DrawSensorRadius(Position, SensorArray.ShortRangeScanDistance, scene);
            TextureManager.Instance.DrawGameObject(this);
            WeaponSystem.Draw(scene);
            DefenseSystem.DrawShields(this);
            DefenseSystem.DrawLive(this);
            SublightEngine.Draw(scene.GameLayer.DebugSystem, this, scene);
        }

        public void Explode(Scene scene)
        {
            ExplosionSheet.Play("destroy");
            IsDestroyed = true;
            Velocity = 0;

            var shakeamount = (500000 - Vector2.Distance(scene.Camera2D.Position, Position)) / 500000;
            if (shakeamount < 0) shakeamount = 0;
            scene.Camera2D.Shake((int)(shakeamount * 100));

            for (int i = 0; i < 5; i++) scene.GameLayer.ItemManager.PopItem(Direction, Position, new Items.Metall());

            return;
        }
    }
}