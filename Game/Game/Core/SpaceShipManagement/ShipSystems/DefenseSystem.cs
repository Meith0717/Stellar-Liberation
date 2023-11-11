// DefenseSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;

namespace StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems
{
    public class DefenseSystem
    {
        private float mMaxShieldForce;
        private float mMaxHullForce;
        private float mActualShieldForce;
        private float mActualHullForce;
        private float mShieldRegeneration;
        private float mHullRegeneration;
        private float mShieldAlpha;

        public double ShildLevel { get { return mActualShieldForce / mMaxShieldForce; } }
        public double HullLevel { get { return mActualHullForce / mMaxHullForce; } }

        public DefenseSystem(float MaxShieldForce, float MaxHullForce, float shieldRegeneration)
        {
            mActualHullForce = mMaxHullForce = MaxHullForce;
            mActualShieldForce = mMaxShieldForce = MaxShieldForce;
            mShieldRegeneration = shieldRegeneration;
        }

        public void Update(GameTime gameTime)
        {
            mShieldAlpha -= 0.1f;
            mShieldAlpha = mShieldAlpha < 0 ? 0 : mShieldAlpha;

            Regenerate(gameTime);
        }

        private void Regenerate(GameTime gameTime)
        {

            if (mActualShieldForce + mShieldRegeneration >= mMaxShieldForce) 
            {
                mActualShieldForce = mMaxShieldForce;
                return;
            };
            mActualShieldForce += mShieldRegeneration / gameTime.ElapsedGameTime.Milliseconds;
        }

        public void GetDamage(int shieldDamage, int hullDamage)
        {
            if (mActualShieldForce > 0)
            {
                mShieldAlpha = 1;
                mActualShieldForce -= shieldDamage;
                return;
            }
            mActualHullForce -= mActualHullForce > 0 ? hullDamage : 0;
        }
    }
}
