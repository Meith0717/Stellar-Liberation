using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameLogik;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.MovementController;
using Galaxy_Explovive.Core.MyMath;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft
{
    public abstract class Spacecraft : InteractiveObject
    {

        public Vector2 TargetPosition { get; set; }
        public bool IsSelect { get; set; } = false;
        public string SelectTexture { get; set; }
        public string NormalTexture { get; set; }
        public double ShieldForce { get; set; } = 0.1f;
        public double HullForce { get; set; } = 0.1f;

        private double mShield = 100;
        private double mHull = 100;

        public void Update(InputState inputState)
        {
            base.UpdateInputs(inputState);
            RegenerateShield();
            if  (inputState.mActionList.Contains(ActionType.Test))
            {
                Hit(5);
            }
            TextureId = IsSelect ? SelectTexture : NormalTexture;
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

            TextureManager.Instance.DrawLine(startPos, startPos + endPos * new Vector2(shield, 1), Color.CornflowerBlue, 8, 1);
            TextureManager.Instance.DrawLine(startPos, startPos + endPos, Color.DarkRed, 8, 0.9f);

            startPos.Y += 10;

            TextureManager.Instance.DrawLine(startPos, startPos + endPos * new Vector2(hull, 1), Color.GreenYellow, 8, 1);
            TextureManager.Instance.DrawLine(startPos, startPos + endPos, Color.DarkRed, 8, 0.9f);

            TextureManager.Instance.Draw(TextureId, Position, TextureOffset,
                TextureWidth, TextureHeight, TextureSclae, Rotation, TextureDepth);
            Crosshair.Draw(Color.White);
            Globals.mDebugSystem.DrawBoundBox(BoundedBox);
        }
    }
}
