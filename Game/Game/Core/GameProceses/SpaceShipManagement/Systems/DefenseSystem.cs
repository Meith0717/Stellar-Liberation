// DefenseSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.ProjectileManagement;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;
using System;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems
{
    [Serializable]
    public class DefenseSystem
    {
        [JsonProperty] private float mMaxShieldForce;
        [JsonProperty] private float mMaxHullForce;
        [JsonProperty] private float mRegenerationPerSecond;

        [JsonIgnore] private float mActualShieldForce;
        [JsonIgnore] private float mActualHullForce;
        [JsonIgnore] private float mShieldDrawAlpha; // Alpah for Drawind Shields by Impact

        [JsonIgnore] private int ShieldRegenerateCoolDown;

        public double ShieldPercentage => mActualShieldForce / mMaxShieldForce;
        public double HullPercentage => mActualHullForce / mMaxHullForce;
        public double ShieldForce => mActualShieldForce;
        public double HullForce => mActualHullForce;

        public DefenseSystem(float MaxShieldForce, float MaxHullForce, float regenerationPerSecond)
        {
            mActualShieldForce = mMaxShieldForce = MaxShieldForce;
            mActualHullForce = mMaxHullForce = MaxHullForce;
            mRegenerationPerSecond = regenerationPerSecond;
        }

        public void Update(GameTime gameTime)
        {
            mShieldDrawAlpha = mShieldDrawAlpha < 0 ? 0 : mShieldDrawAlpha - 0.1f;
            ShieldRegenerateCoolDown -= gameTime.ElapsedGameTime.Milliseconds;

            if (ShieldRegenerateCoolDown > 0) return;

            // Regenerate Shield
            if (mActualShieldForce + mRegenerationPerSecond >= mMaxShieldForce)
            {
                mActualShieldForce = mMaxShieldForce;
                return;
            };
            mActualShieldForce += mRegenerationPerSecond / gameTime.ElapsedGameTime.Milliseconds;
        }

        public void GotHit(Vector2 position, float shieldDamage, float hullDamage, Scene scene)
        {
            ShieldRegenerateCoolDown = 5000;
            if (mActualShieldForce > 0)
            {
                mShieldDrawAlpha = 1f;
                mActualShieldForce -= shieldDamage;
                if (mActualShieldForce < 0) mActualShieldForce = 0;
                return;
            }
            mActualHullForce -= mActualHullForce > 0 ? hullDamage : 0;
            ExplosionEffect.ShipHit(position, Vector2.Zero, scene.ParticleManager);
        }

        public void Upgrade(float shieldUpgradePercentage = 0, float hullUpgradePercentage = 0, float regenerationUpgradePercentage = 0)
        {
            mMaxShieldForce += mMaxShieldForce * shieldUpgradePercentage;
            mMaxHullForce += mMaxHullForce * hullUpgradePercentage;
            mRegenerationPerSecond += mRegenerationPerSecond * regenerationUpgradePercentage;
        }

        public void Repair(float amount)
        {
            if (amount > mMaxHullForce)
            {
                mActualHullForce = mMaxHullForce;
                return;
            }
            mActualHullForce += amount;
        }

        public void DrawShields(SpaceShip spaceShip)
        {
            var color = new Color((int)(87 * mShieldDrawAlpha), (int)(191 * mShieldDrawAlpha), (int)(255 * mShieldDrawAlpha), (int)(255 * mShieldDrawAlpha));
            TextureManager.Instance.Draw($"{spaceShip.TextureId}Shield", spaceShip.Position, spaceShip.TextureScale, spaceShip.Rotation, spaceShip.TextureDepth + 1, color);
        }
    }
}
