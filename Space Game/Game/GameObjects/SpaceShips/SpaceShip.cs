using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public abstract class SpaceShip : GameObject
    {
        public float Velocity { get; internal set; } = 0;
        internal float mMaxVelocity = 50;
        internal List<SpaceShip> mTargets = new();
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
            : base(position, textureId, textureScale, 10)
        {
            mShieldForce = mMaxHullForce;
            mHullForce = mMaxHullForce;
            mInitialWeaponCoolDown = MaxInitialWeaponCoolDown;
            mSecondaryWeaponCoolDown = MaxSecondaryWeaponCoolDown;
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            mSecondaryWeaponCoolDown = (mSecondaryWeaponCoolDown > MaxSecondaryWeaponCoolDown) ?
                MaxSecondaryWeaponCoolDown : mSecondaryWeaponCoolDown + gameTime.ElapsedGameTime.Milliseconds;

            mInitialWeaponCoolDown = (mInitialWeaponCoolDown > MaxInitialWeaponCoolDown) ?
                MaxInitialWeaponCoolDown : mInitialWeaponCoolDown + gameTime.ElapsedGameTime.Milliseconds;

            RemoveFromSpatialHashing(sceneLayer);
            Position += Geometry.CalculateDirectionVector(Rotation) * Velocity * gameTime.ElapsedGameTime.Milliseconds;
            TargetsInRadius(sceneLayer, out mTargets);
            base.Update(gameTime, inputState, sceneLayer);
            AddToSpatialHashing(sceneLayer);
        }

        private bool TargetsInRadius(SceneLayer sceneLayer, out List<SpaceShip> targets)
        {
            targets = sceneLayer.GetSortedObjectsInRadius<SpaceShip>(Position, 1000000);
            targets.Remove(this);
            if (targets.Count == 0) return false;
            return true;
        }

        public bool GetTarget(out SpaceShip target)
        {
            target = null;
            if (mTargets.Count > 0)
            {
                target = mTargets[targetedIndex];
                return true;
            }
            return false;
        }

        public void DoDamage(int shieldDamage, int hullDamage)
        {
            SoundManager.Instance.PlaySound("torpedoHit", Utility.Random.Next(5, 8) / 10f);
            if (mShieldForce > 0)
            {
                mShieldForce -= shieldDamage;
                return;
            }
            mHullForce -= mHullForce > 0 ? hullDamage : 0;
        }

        public double ShildLevel { get { return mShieldForce / mMaxShieldForce; } }
        public double HullLevel { get { return mHullForce / mMaxHullForce; } }
        public double SpeedLevel { get { return Velocity / mMaxVelocity; } }

        public void DrawLive()
        {
            var large = (int)(Math.Max(Width, Height) * TextureScale);
            DrawLevel(large, Position + new Vector2(0, -large / 2), HullLevel, new Color(210, 105, 30));
            DrawLevel(large, Position + new Vector2(0, -large / 2 - 500), ShildLevel, new Color(135, 206, 235));
        }

        private void DrawLevel(int length, Vector2 position, double level, Color color)
        {
            var start = new Vector2(position.X - (length / 2), position.Y);
            TextureManager.Instance.DrawLine(start, length, new Color(39, 39, 39), 300, TextureDepth+1);
            TextureManager.Instance.DrawLine(start, length * (float)level, color, 300, TextureDepth+2);

        }
    }
}
