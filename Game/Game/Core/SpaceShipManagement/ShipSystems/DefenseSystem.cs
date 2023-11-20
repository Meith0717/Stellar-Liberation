// DefenseSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;

namespace StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems
{
    public class DefenseSystem
    {
        private float mMaxShieldForce;
        private float mMaxHullForce;
        private float mActualShieldForce;
        private float mActualHullForce;
        private float mShieldRegeneration;
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

        public void DrawShields(SpaceShip spaceShip)
        {
            var alpha = mShieldAlpha;
            var color = new Color(87, 191, 255);
            TextureManager.Instance.Draw($"{spaceShip.TextureId}Shield", spaceShip.Position,
                spaceShip.TextureScale, spaceShip.Rotation, spaceShip.TextureDepth + 1,
                new Color((int)(color.R * alpha), (int)(color.G * alpha), (int)(color.B * alpha), (int)(255 * alpha)));
        }
    }
}
