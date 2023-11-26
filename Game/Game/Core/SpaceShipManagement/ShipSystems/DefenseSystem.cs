// DefenseSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Core.GameEngine.Content_Management;
using System;

namespace StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems
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
            mShieldDrawAlpha = (mShieldDrawAlpha < 0) ? 0 : mShieldDrawAlpha - 0.1f;

            // Regenerate Shield
            if (mActualShieldForce + mRegenerationPerSecond >= mMaxShieldForce)
            {
                mActualShieldForce = mMaxShieldForce;
                return;
            };
            mActualShieldForce += mRegenerationPerSecond / gameTime.ElapsedGameTime.Milliseconds;
        }

        public void GotHit(int shieldDamage, int hullDamage)
        {
            if (mActualShieldForce > 0)
            {
                mShieldDrawAlpha = 1f;
                mActualShieldForce -= shieldDamage;
                return;
            }
            mActualHullForce -= mActualHullForce > 0 ? hullDamage : 0;
        }

        public void Upgrade(float shieldUpgradePercentage = 0, float hullUpgradePercentage = 0, float regenerationUpgradePercentage = 0)
        {
            mMaxShieldForce += mMaxShieldForce * shieldUpgradePercentage;
            mMaxHullForce += mMaxHullForce * hullUpgradePercentage;
            mRegenerationPerSecond += mRegenerationPerSecond * regenerationUpgradePercentage;
        }

        public void DrawShields(SpaceShip spaceShip)
        {
            var color = new Color((int)(87 * mShieldDrawAlpha), (int)(191 * mShieldDrawAlpha), (int)(255 * mShieldDrawAlpha), (int)(255 * mShieldDrawAlpha));
            TextureManager.Instance.Draw($"{spaceShip.TextureId}Shield", spaceShip.Position,spaceShip.TextureScale, spaceShip.Rotation, spaceShip.TextureDepth + 1, color);
        }
    }
}
