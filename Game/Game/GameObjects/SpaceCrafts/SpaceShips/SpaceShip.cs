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
using StellarLiberation.Game.Core.GameProceses.AI.Behaviors;
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

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips
{
    [Serializable]
    public class SpaceShip : GameObject2D, ICollidable
    {
        [JsonIgnore] private readonly ItemCollector mItemCollector = new();
        [JsonIgnore] private readonly UtilityAi mUtilityAi = new();

        [JsonIgnore] public readonly SensorSystem SensorSystem = new();
        [JsonIgnore] public readonly HyperDrive HyperDrive = new();
        [JsonIgnore] public readonly SublightDrive SublightDrive;
        [JsonIgnore] public readonly PhaserCannons PhaserCannaons;
        [JsonIgnore] public readonly DefenseSystem DefenseSystem;

        [JsonProperty] public readonly Fractions Fraction;
        [JsonProperty] private readonly Color mAccentColor;
        [JsonProperty] public readonly Inventory Inventory = new();
        [JsonProperty] public PlanetSystem PlanetSystem;

        [JsonIgnore] public readonly SpaceShipController mSpaceShipController = new();
        public float Mass { get => 5; } 

        public SpaceShip(Vector2 position, Fractions fraction, SpaceShipConfig config)
            : base(position, config.TextureID, config.TextureScale, 10)
        {
            Fraction = fraction;
            var accentCoor = Fraction switch
            {
                Fractions.Allied => Color.LightBlue,
                Fractions.Enemys => Color.MonoGameOrange,
                Fractions.Neutral => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
            SublightDrive = new(config.Velocity, 0.01f);
            PhaserCannaons = new(config.TurretCoolDown, accentCoor, 10, 10);
            DefenseSystem = new(config.ShieldForce, config.HullForce, 10);
            mAccentColor = accentCoor;
            foreach (var pos in config.WeaponsPositions)
                PhaserCannaons.PlaceTurret(new(pos, 1, TextureDepth + 1));
             
            mUtilityAi.AddBehavior(new IdleBehavior(SublightDrive));
            mUtilityAi.AddBehavior(new ChaseBehavior(this));
            mUtilityAi.AddBehavior(new CollectItemsBehavior(this));
            mUtilityAi.AddBehavior(new CombatBehavior(this));
            mUtilityAi.AddBehavior(new FleeBehavior(this));
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer)
        {
            SublightDrive.Update(gameTime, this, DefenseSystem.HullPercentage);

            MovingDirection = Geometry.CalculateDirectionVector(Rotation);
            Physics.HandleCollision(gameTime, this, gameLayer.SpatialHashing);
            GameObject2DMover.Move(gameTime, this, gameLayer.SpatialHashing);
            base.Update(gameTime, inputState, gameLayer);
            TrailEffect.Show(Transformations.Rotation(Position, new(-100, 0), Rotation), MovingDirection, Velocity, gameTime, mAccentColor, gameLayer.ParticleManager, gameLayer.GameSettings.ParticlesMultiplier);

            SensorSystem.Scan(gameTime, PlanetSystem, Position, Fraction, gameLayer);
            HasProjectileHit(gameTime, gameLayer);
            HyperDrive.Update(gameTime, this, gameLayer);
            DefenseSystem.Update(gameTime);
            mItemCollector.Collect(gameTime, this, gameLayer);
            PhaserCannaons.Update(gameTime, this, gameLayer);
            mUtilityAi.Update(gameTime);

            if (DefenseSystem.HullPercentage <= 0) Explode(gameLayer);
        }

        private void Explode(GameLayer gameLayer)
        {
            ExplosionEffect.ShipDestroyed(Position, gameLayer.ParticleManager, gameLayer.GameSettings.ParticlesMultiplier);
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
            if (projectileInRange.Count == 0) return;
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

            scene.DebugSystem.DrawAiDebug(BoundedBox, mUtilityAi.DebugMessage, scene.Camera2D.Zoom);

            PhaserCannaons.Draw(scene);
            SublightDrive.Draw(scene.DebugSystem, this, scene);
            DefenseSystem.DrawShields(this);
        }
    }
}