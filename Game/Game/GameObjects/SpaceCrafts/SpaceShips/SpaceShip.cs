// SpaceShip.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.AI;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PhaserSystem;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PropulsionSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.Recources.Items;
using System;
using System.Linq;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips
{
    [Serializable]
    public class SpaceShip : GameObject2D, ICollidable
    {
        [JsonIgnore] protected readonly UtilityAi mUtilityAi = new();
        [JsonIgnore] public readonly HyperDrive HyperDrive = new();
        [JsonIgnore] public readonly SensorSystem SensorSystem;
        [JsonIgnore] public readonly SublightDrive SublightDrive;
        [JsonIgnore] public readonly PhaserCannons WeaponSystem;
        [JsonIgnore] public readonly DefenseSystem DefenseSystem;
        [JsonIgnore] public readonly Fractions Fraction;
        [JsonProperty] public readonly Inventory Inventory = new();

        [JsonIgnore] private readonly TractorBeam mTractorBeam;
        [JsonIgnore] private readonly Color mAccentColor;
        [JsonProperty] public PlanetSystem PlanetSystem;

        public float Mass { get => 5; } 

        public SpaceShip(Vector2 position, SpaceShipConfig config)
            : base(position, config.TextureID, config.TextureScale, 10)
        {
            Fraction = config.Fraction;
            var accentCoor = Fraction switch
            {
                Fractions.Allied => Color.LightBlue,
                Fractions.Enemys => Color.MonoGameOrange,
                Fractions.Neutral => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
            SensorSystem = new(config.SensorRange);
            SublightDrive = new(config.Velocity, 0.1f);
            WeaponSystem = new(config.TurretCoolDown, accentCoor, 10, 10, 10000);
            DefenseSystem = new(config.ShieldForce, config.HullForce, 10);
            mTractorBeam = new(2000);
            mAccentColor = accentCoor;
            foreach (var pos in config.WeaponsPositions)
                WeaponSystem.PlaceTurret(new(pos, 1, TextureDepth + 1));
            mUtilityAi = new();
            foreach (var beh in config.AIBehaviors)
                mUtilityAi.AddBehavior(beh);
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer)
        {
            HyperDrive.Update(gameTime, this, gameLayer);
            if (!HyperDrive.IsActive) SublightDrive.Update(gameTime, this);

            MovingDirection = Geometry.CalculateDirectionVector(Rotation);
            Physics.HandleCollision(gameTime, this, gameLayer.SpatialHashing);
            GameObject2DMover.Move(gameTime, this, gameLayer.SpatialHashing);
            base.Update(gameTime, inputState, gameLayer);

            HasProjectileHit(gameTime, gameLayer);
            DefenseSystem.Update(gameTime);
            SensorSystem.Scan(gameTime, PlanetSystem, Position, Fraction, gameLayer);
            WeaponSystem.Update(gameTime, this, gameLayer);
            mUtilityAi.Update(gameTime, this, gameLayer);
            mTractorBeam.Pull(gameTime, this, gameLayer);
            TrailEffect.Show(Transformations.Rotation(Position, new(-100, 0), Rotation), MovingDirection, Velocity, gameTime, mAccentColor, gameLayer.ParticleManager, gameLayer.GameSettings.ParticlesMultiplier);

            if (DefenseSystem.HullPercentage > 0) return;
            ExplosionEffect.ShipDestroyed(Position, MovingDirection, gameLayer.ParticleManager, gameLayer.GameSettings.ParticlesMultiplier);
            IsDisposed = true;
            var distance = Vector2.Distance(gameLayer.Camera2D.Position, Position);
            var threshold = MathHelper.Clamp(1 - (distance / 7500), 0, 1);
            gameLayer.CameraShaker.Shake(200 * threshold, 1);

            for (int i = 0; i < ExtendetRandom.Random.Next(0, 5); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var vector);
                PlanetSystem.GameObjects.Add(ItemFactory.Get(ItemID.Iron, MovingDirection + vector * 5, Position));
            }
        }

        private void HasProjectileHit(GameTime gameTime, GameLayer scene)
        {
            var projectileInRange = scene.SpatialHashing.GetObjectsInRadius<LaserProjectile>(Position, (int)BoundedBox.Radius * 10);
            if (!projectileInRange.Any()) return;
            var hit = false;
            foreach (var projectile in projectileInRange)
            {
                if (projectile.Fraction == Fraction) continue;
                if (!ContinuousCollisionDetection.HasCollide(gameTime, projectile, this, out var position)) continue;

                projectile.HasCollide((Vector2)position, scene);
                DefenseSystem.GotHit((Vector2)position, projectile.ShieldDamage, projectile.HullDamage, scene);
                hit = true;
            }
            if (!hit) return;
            SoundEffectSystem.PlaySound(SoundEffectRegistries.torpedoHit, scene.Camera2D, Position);
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);

            scene.DebugSystem.DrawSensorRadius(Position, SensorSystem.ShortRangeScanDistance, scene);

            TextureManager.Instance.Draw($"{TextureId}Borders", Position, TextureScale, Rotation, TextureDepth, mAccentColor);
            TextureManager.Instance.Draw($"{TextureId}Frame", Position, TextureScale, Rotation, TextureDepth, Color.Black);
            TextureManager.Instance.Draw($"{TextureId}Hull", Position, TextureScale, Rotation, TextureDepth, new(10, 10, 10));
            TextureManager.Instance.Draw($"{TextureId}Structure", Position, TextureScale, Rotation, TextureDepth, new(20, 30, 40));
            TextureManager.Instance.Draw(GameSpriteRegistries.radar, Position, .04f / scene.Camera2D.Zoom, 0, TextureDepth + 1,  Fraction == Fractions.Enemys ? Color.Red : Color.LightGreen);

            mTractorBeam.Draw(this);
            scene.DebugSystem.DrawAiDebug(BoundedBox, mUtilityAi.DebugMessage, scene.Camera2D.Zoom);

            WeaponSystem.Draw(scene);
            SublightDrive.Draw(scene.DebugSystem, this, scene);
            DefenseSystem.DrawShields(this);
        }
    }
}