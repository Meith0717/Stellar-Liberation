using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.BattleSystem.WeaponSystem
{
    public abstract class Projectile : GameObject
    {
        internal SpaceShip mTargetObj { get; private set; }
        internal Vector2 mVariance { get; private set; }
        internal float mVelocity { get; private set; }

        public float LiveTime = 60000;
        private int mShieldDamage;
        private int mHullDamage;

        internal Projectile(Vector2 position, SpaceShip targetObj, string textureId, float textureScale, int shieldDamage, int hullDamage, float velocity)
            : base(position, textureId, textureScale, 1)
        {
            mVelocity = velocity;
            var variance = (int)(Math.Min(targetObj.Width, targetObj.Height) * 0.5f);
            mVariance = Utility.GetRandomVector2(-variance, variance, -variance, variance);
            mTargetObj = targetObj;
            mShieldDamage = shieldDamage;
            mHullDamage = hullDamage;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            base.Update(gameTime, inputState, engine);
            LiveTime -= gameTime.ElapsedGameTime.Milliseconds;
            CheckForHit();
        }

        internal void CheckForHit()
        {
            if (mTargetObj == null) return;
            if (!mTargetObj.BoundedBox.Contains(Position)) return;
            mTargetObj.DoDamage(mShieldDamage, mHullDamage);
            LiveTime = 0;
        }
    }
}
