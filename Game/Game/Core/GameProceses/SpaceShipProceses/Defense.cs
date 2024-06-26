// Defense.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses
{
    [Serializable]
    public class Defense
    {
        // Shield Stuff
        [JsonProperty] private float mActualShieldForce;
        [JsonProperty] public float MaxShieldForce { get; private set; }
        [JsonProperty] public float ShieldRegenerationPerSecond { get; private set; }
        [JsonIgnore] public double ShieldPercentage => mActualShieldForce / MaxShieldForce;

        // Hull Stuff
        [JsonProperty] private float mActualHullForce;
        [JsonProperty] public float MaxHullForce { get; private set; }
        [JsonProperty] public float HullRegenerationPerSecond { get; private set; }
        [JsonIgnore] public double HullPercentage => mActualHullForce / MaxHullForce;

        // Update Stuff
        [JsonIgnore] private int mRegenerateCoolDown;
        [JsonIgnore] private float mShieldDrawAlpha;

        public Defense(float shieldForce, float hullForce, float shieldReg, float hullReg)
        {
            MaxShieldForce = shieldForce;
            MaxHullForce = hullForce;
            ShieldRegenerationPerSecond = shieldReg;
            HullRegenerationPerSecond = hullReg;
            mActualHullForce = MaxHullForce;
            mActualShieldForce = MaxShieldForce;
        }

        public void Boost(float shieldForcePerc, float hullForcePerc, float shieldRegPerc, float hullRegPerc)
        {
            MaxShieldForce *= shieldForcePerc;
            MaxHullForce *= hullForcePerc;
            ShieldRegenerationPerSecond *= shieldRegPerc;
            HullRegenerationPerSecond *= hullRegPerc;
        }

        public void Update(Spacecraft spacecraft, GameTime gameTime, GameSettings gameSettings, PlanetsystemState planetsystemState)
        {
            mShieldDrawAlpha = mShieldDrawAlpha < 0 ? 0 : mShieldDrawAlpha - 0.1f;
            mRegenerateCoolDown -= gameTime.ElapsedGameTime.Milliseconds;

            CheckForHit(spacecraft, gameTime, gameSettings, planetsystemState);

            if (mRegenerateCoolDown > 0) return;
            mActualShieldForce += ShieldRegenerationPerSecond / (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float.Clamp(mActualShieldForce, 0, MaxShieldForce);
            mActualHullForce += HullRegenerationPerSecond / (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float.Clamp(mActualHullForce, 0, MaxHullForce);
        }

        public void GotHit(int shieldDamage, int hullDamage)
        {
            mRegenerateCoolDown = 5000;
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
            planetsystemState.StereoSounds.Enqueue(new(spacecraft.Position, "torpedoHit"));
        }
    }
}
