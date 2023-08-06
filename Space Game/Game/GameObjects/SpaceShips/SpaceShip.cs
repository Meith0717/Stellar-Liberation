using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.WeaponSystem;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public abstract class SpaceShip : GameObject
    {
        public float Velocity { get; internal set; } = 0;
        internal float mMaxVelocity = 5;
        internal List<GameObject> mTargets = new();
        internal int targetedIndex = 0;

        public bool HasTarget { get; internal set; }
        public int MaxInitialWeaponCoolDown { get; internal set; } = 500;
        private int mInitialWeaponCoolDown;
        public int MaxSecondaryWeaponCoolDown { get; internal set; } = 500;
        private int mSecondaryWeaponCoolDown;


        public float mShieldForce { get; private set; }
        public float mHullForce { get; private set; }
        private float mMaxShieldForce = 100;
        private float mMaxHullForce = 100;


        public SpaceShip(Vector2 position, string textureId, float textureScale)
            : base(position, textureId, textureScale, 2)
        {
            mShieldForce = mMaxHullForce;
            mHullForce = mMaxHullForce;
            mInitialWeaponCoolDown = MaxInitialWeaponCoolDown;
            mSecondaryWeaponCoolDown = MaxSecondaryWeaponCoolDown;
        }

        public virtual void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine gameEngine, WeaponManager weaponManager)
        {
            mSecondaryWeaponCoolDown = (mSecondaryWeaponCoolDown > MaxSecondaryWeaponCoolDown) ? 
                MaxSecondaryWeaponCoolDown : mSecondaryWeaponCoolDown + gameTime.ElapsedGameTime.Milliseconds;

            mInitialWeaponCoolDown = (mInitialWeaponCoolDown > MaxInitialWeaponCoolDown) ? 
                MaxInitialWeaponCoolDown : mInitialWeaponCoolDown + gameTime.ElapsedGameTime.Milliseconds;
            
            RemoveFromSpatialHashing(gameEngine);
            Position += Geometry.CalculateDirectionVector(Rotation) * Velocity * gameTime.ElapsedGameTime.Milliseconds;
            ChechForHit(gameEngine);
            TargetsInRadius(gameEngine, out mTargets);
            base.Update(gameTime, inputState, gameEngine);
            AddToSpatialHashing(gameEngine);
        }

        [Obsolete("This method is deprecated. Use other Update instead.", true)]
        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine gameEngine) 
        {
            throw new Exception("This method is deprecated. Use other Update instead.");
        }

        internal bool GetTarget(List<GameObject> targets, out GameObject target)
        {
            target = null;
            if (targets.Count > 0)
            {
                target = targets[targetedIndex];
                return true;
            }
            return false;
        }

        internal void FireSecondaryWeapon(WeaponManager weaponManager)
        {
            if (!GetTarget(mTargets, out var target)) return;
            if (mSecondaryWeaponCoolDown < MaxSecondaryWeaponCoolDown) return; 
            weaponManager.AddTorpedo(this, target);
            SoundManager.Instance.PlaySound("torpedoFire", Utility.Random.Next(5, 8)/10f);
            mSecondaryWeaponCoolDown = 0;
        }

        internal void FireInitialWeapon(WeaponManager weaponManager)
        {
            if (!GetTarget(mTargets, out var target)) return;
            if (mInitialWeaponCoolDown < MaxInitialWeaponCoolDown) return;
            weaponManager.AddLaser(this, target);
            mInitialWeaponCoolDown = 0;
        }


        internal void ChechForHit(GameEngine.GameEngine engine)
        {
            List<Weapon> weapons = engine.GetObjectsInRadius<Weapon>(Position, 100);
            foreach (var item in weapons)
            {
                if (!BoundedBox.Contains(item.Position)) continue;
                item.LiveTime = 0;
                SoundManager.Instance.PlaySound("torpedoHit", Utility.Random.Next(5, 10) / 10f);
                DoDamage(10);
            }
        }

        private void DoDamage(int damage)
        {
            if (mShieldForce > 0)
            {
                mShieldForce -= damage;
                return;
            }
            mHullForce -= mHullForce > 0 ? damage : 0;
        }

        internal bool TargetsInRadius(GameEngine.GameEngine gameEngine, out List<GameObject> targets) 
        {
            targets = gameEngine.ObjectsOnScreen.OfType<GameObject>().ToList();
            targets.Remove(this);
            if (targets.Count == 0) return false;
            return true;
        }

        public double ShildLevel { get { return mShieldForce / mMaxShieldForce; } }
        public double HullLevel { get { return mHullForce / mMaxHullForce; } }
        public double SpeedLevel { get { return Velocity / mMaxVelocity; } }

    }
}
