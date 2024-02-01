// SpaceShip.cs 
// Copyright (c) 2023 Thierry Meiers 
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
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PhaserSystem;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PropulsionSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.Recources.Items;
using System;
using System.Linq;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips
{
    [Serializable]
    [Collidable(5f)]
    public class SpaceShip : GameObject2D
    {
        [JsonIgnore] protected readonly UtilityAi mUtilityAi = new();
        [JsonIgnore] public readonly HyperDrive HyperDrive = new();

        [JsonIgnore] public readonly Fractions Fraction;
        [JsonIgnore] public readonly SensorSystem SensorSystem;
        [JsonIgnore] public readonly SublightDrive SublightDrive;
        [JsonIgnore] public readonly PhaserCannons WeaponSystem;
        [JsonIgnore] public readonly DefenseSystem DefenseSystem;
        [JsonIgnore] private readonly Color mAccentColor;

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
            mAccentColor = accentCoor;
            foreach (var pos in config.WeaponsPositions)
                WeaponSystem.PlaceTurret(new(pos, 1, TextureDepth + 1));
            mUtilityAi = new();
            foreach (var beh in config.AIBehaviors)
                mUtilityAi.AddBehavior(beh);
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            HyperDrive.Update(gameTime, this, scene);
            if (!HyperDrive.IsActive) SublightDrive.Update(gameTime, this);

            MovingDirection = Geometry.CalculateDirectionVector(Rotation);
            Physics.HandleCollision(gameTime, this, scene.SpatialHashing);
            GameObject2DMover.Move(gameTime, this, scene);
            base.Update(gameTime, inputState, scene);

            HasProjectileHit(gameTime, scene);
            DefenseSystem.Update(gameTime);
            SensorSystem.Scan(Position, Fraction, scene);
            WeaponSystem.Update(gameTime, this, scene);
            mUtilityAi.Update(gameTime, this, scene);
            TrailEffect.Show(Transformations.Rotation(Position, new(-100, 0), Rotation), MovingDirection, Velocity, gameTime, mAccentColor, scene.ParticleManager);

            if (DefenseSystem.HullPercentage > 0) return;
            ExplosionEffect.ShipDestroyed(Position, scene.ParticleManager);
            Dispose = true;
            for (int i = 0; i < ExtendetRandom.Random.Next(0, 5); i++)
            {
                ExtendetRandom.Random.NextUnitVector(out var vector);
                scene.GameState.CurrentSystem.GameObjectManager.AddObj(ItemFactory.Get(ItemID.Iron, MovingDirection + vector * 5, Position));
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

            scene.GameState.DebugSystem.DrawSensorRadius(Position, SensorSystem.ShortRangeScanDistance, scene);

            TextureManager.Instance.Draw($"{TextureId}Borders", Position, TextureScale, Rotation, TextureDepth, mAccentColor);
            TextureManager.Instance.Draw($"{TextureId}Frame", Position, TextureScale, Rotation, TextureDepth, Color.Black);
            TextureManager.Instance.Draw($"{TextureId}Hull", Position, TextureScale, Rotation, TextureDepth, new(10, 10, 10));
            TextureManager.Instance.Draw($"{TextureId}Structure", Position, TextureScale, Rotation, TextureDepth, new(20, 30, 40));

            scene.GameState.DebugSystem.DrawAiDebug(BoundedBox, mUtilityAi.DebugMessage, scene.Camera2D.Zoom);

            WeaponSystem.Draw(scene);
            SublightDrive.Draw(scene.GameState.DebugSystem, this, scene);
            DefenseSystem.DrawShields(this);
        }
    }
}