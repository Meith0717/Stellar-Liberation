﻿using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Game.Layers;
using Microsoft.Xna.Framework;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft
{
    public abstract class Spacecraft : InteractiveObject
    {

        public bool IsSelect { get; set; } = false;
        public string SelectTexture { get; set; }
        public string NormalTexture { get; set; }
        public double ShieldForce { get; set; } = 0.1f;
        public double HullForce { get; set; } = 0.1f;

        private double mShield = 100;
        private double mHull = 100;

        protected Spacecraft(GameLayer gameLayer) : base(gameLayer) { }

        public override void UpdateLogik(GameTime gameTime, InputState inputState)
        {
            base.UpdateLogik(gameTime, inputState);
            TextureId = NormalTexture;
            RegenerateShield();
        }

        // Fight Stuff
        private void RegenerateShield()
        {
            if (mHull == 0) { return; }
            double reg = 1 * ShieldForce;
            if (mShield >= 100) { mShield = 100; return; }
            mShield += reg;
        }
        public void RegenerateHull()
        {
            if (mHull == 0) { return; }
            double reg = 1 * HullForce;
            if (mHull >= 100) { mHull = 100; return; }
            mHull += reg;
        }
        public void Hit(int damage)
        {
            double ShieldDamage = damage * (1 - ShieldForce);
            double HulldDamage = damage * (1 - HullForce);

            if (mShield - ShieldDamage > 0.01f) 
            { 
                mShield -= ShieldDamage; 
                return; 
            }
            mShield = 0;
            if (mHull - HulldDamage < 0.01f) { mHull = 0; return; }
            mHull -= HulldDamage;
        }
        
        public void DrawSpaceCraft()
        {
            Vector2 startPos = Position - TextureOffset - new Vector2(0, 50);
            Vector2 endPos = new Vector2(TextureOffset.X * 2, 0);
            float hull = (float)mHull / 100;
            float shield = (float)mShield / 100;

            mTextureManager.DrawLine(startPos, startPos + endPos * new Vector2(shield, 1), Color.CornflowerBlue, 8, 1);
            mTextureManager.DrawLine(startPos, startPos + endPos, Color.DarkRed, 8, 0.9f);

            startPos.Y += 10;

            mTextureManager.DrawLine(startPos, startPos + endPos * new Vector2(hull, 1), Color.GreenYellow, 8, 1);
            mTextureManager.DrawLine(startPos, startPos + endPos, Color.DarkRed, 8, 0.9f);

            mTextureManager.DrawGameObject(this, IsHover);
            mGameLayer.mDebugSystem.DrawBoundBox(mTextureManager, BoundedBox);
        }
    }
}
