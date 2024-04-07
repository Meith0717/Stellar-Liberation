﻿// DefenseSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.SpaceShipComponents.Weapons;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using StellarLiberation.Game.Layers;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipComponents
{
    [Serializable]
    public class Defense
    {
        [JsonProperty] private readonly float mMaxShieldForce;
        [JsonProperty] private readonly float mShieldRegenerationPerSecond;
        [JsonProperty] private readonly float mMaxHullForce;
        [JsonProperty] private readonly float mHullRegenerationPerSecond;

        [JsonProperty] private float mActualShieldForce;
        [JsonProperty] private float mActualHullForce;
        [JsonIgnore] private int RegenerateCoolDown;
        [JsonIgnore] private float mShieldDrawAlpha;

        [JsonIgnore]
        public double ShieldPercentage => mActualShieldForce / mMaxShieldForce;
        [JsonIgnore]
        public double HullPercentage => mActualHullForce / mMaxHullForce;

        public Defense(float shieldForcePerc, float hullForcePerc, float shieldRegPerc, float hullRegPerc)
        {
            mMaxShieldForce = 100 * shieldForcePerc;
            mMaxHullForce = 100 * hullForcePerc;
            mShieldRegenerationPerSecond = 100 * shieldRegPerc;
            mHullRegenerationPerSecond = 100 * hullRegPerc;
            mActualHullForce = mMaxHullForce;
            mActualShieldForce = mMaxShieldForce;
        }

        public void Update(Spacecraft spacecraft, GameTime gameTime, GameSettings gameSettings, PlanetsystemState planetsystemState)
        {
            mShieldDrawAlpha = mShieldDrawAlpha < 0 ? 0 : mShieldDrawAlpha - 0.1f;
            RegenerateCoolDown -= gameTime.ElapsedGameTime.Milliseconds;

            CheckForHit(spacecraft, gameTime, gameSettings, planetsystemState);

            if (RegenerateCoolDown > 0) return;
            mActualShieldForce += mShieldRegenerationPerSecond / (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float.Clamp(mActualShieldForce, 0, mMaxShieldForce);
            mActualHullForce += mHullRegenerationPerSecond / (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float.Clamp(mActualHullForce, 0, mMaxHullForce);
        }

        public void GotHit(int shieldDamage, int hullDamage)
        {
            RegenerateCoolDown = 5000;
            if (mActualShieldForce > 0)
            {
                mShieldDrawAlpha = 1f;
                mActualShieldForce -= shieldDamage;
                if (mActualShieldForce < 0) mActualShieldForce = 0;
                return;
            }
            mActualHullForce -= mActualHullForce > 0 ? hullDamage : 0;
        }

        public void Draw(Spacecraft scpacecraft)
        {
            var color = new Color((int)(255 * mShieldDrawAlpha), (int)(255 * mShieldDrawAlpha), (int)(255 * mShieldDrawAlpha), (int)(255 * mShieldDrawAlpha));
            TextureManager.Instance.Draw($"{scpacecraft.TextureId}Shield", scpacecraft.Position, scpacecraft.TextureScale, scpacecraft.Rotation, scpacecraft.TextureDepth + 1, color);
        }

        private void CheckForHit(Spacecraft spacecraft, GameTime gameTime, GameSettings gameSettings, PlanetsystemState planetsystemState)
        {
            var projectileInRange = planetsystemState.SpatialHashing.GetObjectsInRadius<WeaponProjectile>(spacecraft.Position, (int)spacecraft.BoundedBox.Radius * 10);
            if (projectileInRange.Count == 0) return;
            var hit = false;
            foreach (var projectile in projectileInRange)
            {
                if (projectile.Shooter.Fraction == spacecraft.Fraction) continue;
                if (!ContinuousCollisionDetection.HasCollide(gameTime, projectile, spacecraft, out var position)) continue;

                projectile.HasCollide((Vector2)position, null);
                GotHit((int)projectile.ShieldDamage, (int)projectile.HullDamage); // TODO int
                hit = true;
                ExplosionEffect.ShipHit((Vector2)position, Vector2.Zero, planetsystemState.ParticleEmitors, gameSettings.ParticlesMultiplier);
            }
            if (!hit) return;
            planetsystemState.StereoSounds.Enqueue(new(spacecraft.Position, SoundEffectRegistries.torpedoHit));
        }
    }
}