// SpaceShip.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Random;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.AI;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components;
using StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components.PropulsionSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.Layers;
using System;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships
{
    [Serializable]
    public class Spaceship : GameObject2D, ICollidable
    {
        [JsonIgnore] private readonly ItemCollector mItemCollector = new();
        [JsonIgnore] private readonly UtilityAi mUtilityAi = new();

        [JsonIgnore] public readonly SensorSystem SensorSystem = new();
        [JsonIgnore] public readonly HyperDrive HyperDrive = new();
        [JsonIgnore] public readonly SublightDrive SublightDrive;
        [JsonIgnore] public readonly DefenseSystem DefenseSystem;

        [JsonProperty] public readonly Fractions Fraction;
        [JsonProperty] private readonly Color mAccentColor;
        [JsonProperty] public readonly Inventory Inventory = new();
        [JsonProperty] public readonly int ID;

        [JsonIgnore] public readonly SpaceshipController mSpaceshipController = new();
        public float Mass { get => 5; }

        public Spaceship(Vector2 position, Fractions fraction, string textureID, float textureScale)
            : base(position, textureID, textureScale, 10)
        {
            Fraction = fraction;
            var accentCoor = Fraction switch
            {
                Fractions.Allied => Color.LightBlue,
                Fractions.Enemys => Color.MonoGameOrange,
                Fractions.Neutral => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
            SublightDrive = new(1, 0.01f);
            DefenseSystem = new(1000, 100, 10);
            mAccentColor = accentCoor;

            ID = ExtendetRandom.Random.NextFullRangeInt32();
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            SublightDrive.Update(gameTime, this, DefenseSystem.HullPercentage);

            MovingDirection = Geometry.CalculateDirectionVector(Rotation);
            Physics.HandleCollision(gameTime, this, planetsystemState.SpatialHashing);
            GameObject2DMover.Move(gameTime, this, planetsystemState.SpatialHashing);
            base.Update(gameTime, gameState, planetsystemState);
            TrailEffect.Show(Transformations.Rotation(Position, new(-100, 0), Rotation), MovingDirection, Velocity, gameTime, mAccentColor, planetsystemState.ParticleEmitors, gameState.GameSettings.ParticlesMultiplier);

            SensorSystem.Scan(gameTime, this, Fraction, planetsystemState);
            HasProjectileHit(gameTime, gameState, planetsystemState);
            HyperDrive.Update(gameTime, this, planetsystemState);
            DefenseSystem.Update(gameTime);
            mItemCollector.Collect(gameTime, this, planetsystemState);
            mUtilityAi.Update(gameTime);

            if (DefenseSystem.HullPercentage <= 0) Explode(gameState, planetsystemState);
        }

        private void Explode(GameState gameState, PlanetsystemState gameLayer)
        {
            ExplosionEffect.ShipDestroyed(Position, gameLayer.ParticleEmitors, gameState.GameSettings.ParticlesMultiplier);
            IsDisposed = true;
            // TODO Cam Shaker
        }

        private void HasProjectileHit(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            var projectileInRange = planetsystemState.SpatialHashing.GetObjectsInRadius<LaserProjectile>(Position, (int)BoundedBox.Radius * 10);
            if (projectileInRange.Count == 0) return;
            var hit = false;
            foreach (var projectile in projectileInRange)
            {
                if (projectile.Fraction == Fraction) continue;
                if (!ContinuousCollisionDetection.HasCollide(gameTime, projectile, this, out var position)) continue;

                projectile.HasCollide((Vector2)position, null);
                DefenseSystem.GotHit((Vector2)position, projectile.ShieldDamage, projectile.HullDamage, gameState, planetsystemState);
                hit = true;
            }
            if (!hit) return;
            planetsystemState.StereoSounds.Enqueue(new(Position, SoundEffectRegistries.torpedoHit));
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);

            gameState.DebugSystem.DrawSensorRadius(Position, SensorSystem.ShortRangeScanDistance, scene);

            TextureManager.Instance.Draw($"{TextureId}Borders", Position, TextureScale, Rotation, TextureDepth, mAccentColor);
            TextureManager.Instance.Draw($"{TextureId}Frame", Position, TextureScale, Rotation, TextureDepth, Color.Black);
            TextureManager.Instance.Draw($"{TextureId}Hull", Position, TextureScale, Rotation, TextureDepth, new(10, 10, 10));
            TextureManager.Instance.Draw($"{TextureId}Structure", Position, TextureScale, Rotation, TextureDepth, new(20, 30, 40));

            gameState.DebugSystem.DrawAiDebug(BoundedBox, mUtilityAi.DebugMessage, scene.Camera2D.Zoom);

            SublightDrive.Draw(gameState.DebugSystem, this, scene);
            DefenseSystem.DrawShields(this);
        }
    }
}