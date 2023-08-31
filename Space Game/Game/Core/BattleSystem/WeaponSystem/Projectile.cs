using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.BattleSystem.WeaponSystem
{
    public abstract class Projectile : GameObject
    {
        internal SpaceShip mTargetObj { get; private set; }
        internal Vector2 mStartPosition { get; private set; }
        internal Vector2 mVariance { get; private set; }
        internal float mVelocity { get; private set; }

        public float LiveTime = 60000;
        private int mShieldDamage;
        private int mHullDamage;

        internal Projectile(Vector2 startPosition, SpaceShip targetObj, string textureId, float textureScale, int shieldDamage, int hullDamage, float velocity)
            : base(startPosition, textureId, textureScale, 10)
        {
            mVelocity = velocity;
            var variance = (int)(Math.Min(targetObj.Width, targetObj.Height) * 0.5f);
            mVariance = Utility.Utility.GetRandomVector2(-variance, variance, -variance, variance);
            mTargetObj = targetObj;
            mShieldDamage = shieldDamage;
            mHullDamage = hullDamage;
        }

        public virtual void Update(Vector2 startPosition, GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            base.Update(gameTime, inputState, sceneLayer);
            mStartPosition = startPosition;
            LiveTime -= gameTime.ElapsedGameTime.Milliseconds;
            CheckForHit();
        }

        [Obsolete]
        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            throw new Exception();
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
