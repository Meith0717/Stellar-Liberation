using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.ShipSystems
{
    public class DefenseSystem
    {
        private float mMaxShieldForce;
        private float mMaxHullForce;
        private float mActualShieldForce;
        private float mActualHullForce;
        private float mShieldRegeneration;
        private float mHullRegeneration;

        public double ShildLevel { get { return mActualShieldForce / mMaxShieldForce; } }
        public double HullLevel { get { return mActualHullForce / mMaxHullForce; } }

        public DefenseSystem(float MaxShieldForce, float MaxHullForce, float hullRegeneration, float shieldRegeneration)
        {
            mActualHullForce = mMaxHullForce = MaxHullForce;
            mActualShieldForce = mMaxShieldForce = MaxShieldForce;
            mHullRegeneration = hullRegeneration;
            mShieldRegeneration = shieldRegeneration;
        }

        public void Update(GameTime gameTime)
        {
            if (mActualShieldForce + mShieldRegeneration < mMaxShieldForce) mActualShieldForce += mShieldRegeneration / 
                    gameTime.ElapsedGameTime.Milliseconds;
            if (mActualHullForce + mHullRegeneration < mMaxHullForce) mActualHullForce += mHullRegeneration /
                    gameTime.ElapsedGameTime.Milliseconds;
           
        }

        public void GetDamage(int shieldDamage, int hullDamage)
        {
            SoundManager.Instance.PlaySound("torpedoHit", Utility.Utility.Random.Next(5, 8) / 10f);
            if (mActualShieldForce > 0)
            {
                mActualShieldForce -= shieldDamage;
                return;
            }
            mActualHullForce -= mActualHullForce > 0 ? hullDamage : 0;
        }


        public void DrawLive(SpaceShip spaceShip)
        {
            var large = (int)(Math.Max(spaceShip.Width, spaceShip.Height) * spaceShip.TextureScale);
            DrawLevel(large, spaceShip.Position + new Vector2(0, -large / 2), HullLevel, new Color(210, 105, 30), spaceShip.TextureDepth);
            DrawLevel(large, spaceShip.Position + new Vector2(0, -large / 2 - 500), ShildLevel, new Color(135, 206, 235), spaceShip.TextureDepth);
        }

        private void DrawLevel(int length, Vector2 position, double level, Color color, int textureDepth)
        {
            var start = new Vector2(position.X - (length / 2), position.Y);
            TextureManager.Instance.DrawLine(start, length, new Color(39, 39, 39), 300, textureDepth + 1);
            TextureManager.Instance.DrawLine(start, length * (float)level, color, 300, textureDepth + 2);
        }


    }
}
