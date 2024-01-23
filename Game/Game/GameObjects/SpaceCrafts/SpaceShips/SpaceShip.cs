// SpaceShip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.AI;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.ProjectileManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.PropulsionSystem;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.WeaponSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
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
        [JsonIgnore] protected readonly UtilityAi mUtilityAi;
        [JsonIgnore] public readonly SensorSystem SensorArray;
        [JsonIgnore] public readonly SublightDrive SublightEngine;
        [JsonIgnore] public readonly HyperDrive HyperDrive;
        [JsonIgnore] public readonly TurretSystem WeaponSystem;
        [JsonIgnore] public readonly DefenseSystem DefenseSystem;
        [JsonIgnore] public readonly Fractions Fraction;
        [JsonIgnore] private readonly Color mHullColor;
        [JsonIgnore] private readonly Color mBorderColor;

        public SpaceShip(Vector2 position, string TextureID, float textureScale, SensorSystem sensorArray, SublightDrive sublightEngine, TurretSystem weaponSystem, DefenseSystem defenseSystem, Fractions fractions, Color borderColor, Color hullColor)
            : base(position, TextureID, textureScale, 10)
        {
            mUtilityAi = new();
            SensorArray = sensorArray;
            SublightEngine = sublightEngine;
            HyperDrive = new(500);
            WeaponSystem = weaponSystem;
            DefenseSystem = defenseSystem;
            Fraction = fractions;
            mHullColor = hullColor;
            mBorderColor = borderColor;
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            HyperDrive.Update(gameTime, this, scene);
            if (!HyperDrive.IsActive) SublightEngine.Update(gameTime, this);

            MovingDirection = Geometry.CalculateDirectionVector(Rotation);
            ContinuousCollisionDetection.ManageColisions(gameTime, this, scene.SpatialHashing);
            GameObject2DMover.Move(gameTime, this, scene);
            base.Update(gameTime, inputState, scene);

            HasProjectileHit(gameTime, scene);
            DefenseSystem.Update(gameTime);
            SensorArray.Scan(Position, Fraction, scene);
            WeaponSystem.Update(gameTime, this, scene.GameLayer.CurrentSystem.GameObjectManager);
            mUtilityAi.Update(gameTime, this, scene);

            if (DefenseSystem.HullPercentage > 0) return;
            ExplosionEffect.ShipDestroyed(Position, scene.ParticleManager);
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

            scene.GameLayer.DebugSystem.DrawSensorRadius(Position, SensorArray.ShortRangeScanDistance, scene);

            TextureManager.Instance.Draw($"{TextureId}Borders", Position, TextureScale, Rotation, TextureDepth, mBorderColor);
            TextureManager.Instance.Draw($"{TextureId}Frame", Position, TextureScale, Rotation, TextureDepth, Color.Black);
            TextureManager.Instance.Draw($"{TextureId}Hull", Position, TextureScale, Rotation, TextureDepth, mHullColor);
            TextureManager.Instance.Draw($"{TextureId}Structure", Position, TextureScale, Rotation, TextureDepth, new(27, 38, 49));

            WeaponSystem.Draw(scene);
            SublightEngine.Draw(scene.GameLayer.DebugSystem, this, scene);
            DefenseSystem.DrawShields(this);
        }
    }
}