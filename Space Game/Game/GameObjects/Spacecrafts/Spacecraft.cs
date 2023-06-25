﻿using Galaxy_Explovive.Core;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft
{
    [Serializable]
    public abstract class Spacecraft : InteractiveObject
    {
        [JsonIgnore] public string SelectTexture { get; set; }
        [JsonIgnore] public string NormalTexture { get; set; }
        [JsonProperty] public double ShieldForce { get; set; } = 0.1f;
        [JsonProperty] public double HullForce { get; set; } = 0.1f;

        [JsonProperty] private double mShield = 100;
        [JsonProperty] private double mHull = 100;

        protected Spacecraft() : base() { }

        public override void UpdateLogik(GameTime gameTime, InputState inputState, GameEngine engine)
        {
            base.UpdateLogik(gameTime, inputState, engine);
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
        public void DrawLife(TextureManager textureManager)
        {
            Vector2 startPos = Position - TextureOffset - new Vector2(0, 50);
            Vector2 endPos = new Vector2(TextureOffset.X * 2, 0);
            float hull = (float)mHull / 100;
            float shield = (float)mShield / 100;
            textureManager.DrawLine(startPos, startPos + endPos * new Vector2(shield, 1), Color.CornflowerBlue, 8, 1);
            textureManager.DrawLine(startPos, startPos + endPos, Color.DarkRed, 8, 0.9f);
            startPos.Y += 10;
            textureManager.DrawLine(startPos, startPos + endPos * new Vector2(hull, 1), Color.GreenYellow, 8, 1);
            textureManager.DrawLine(startPos, startPos + endPos, Color.DarkRed, 8, 0.9f);
        }

    }
}
