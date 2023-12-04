// TurretSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.WeaponSystem
{
    public class TurretSystem
    {
        private readonly List<Turret> mTurrets = new();
        private readonly int mMaxFireCoolDown;
        private float mFireCoolDown;
        private Color mParticleColor;
        private float mHullDamage;
        private float mShielDamage;
        public float Range;

        private SpaceShip mAimingShip;
        private Vector2? mAimingPos;
        private bool mFire;

        public TurretSystem(int fireCoolDown, Color particleColor, int hullDamage, int shieldDamage, float range)
        {
            mMaxFireCoolDown = fireCoolDown;
            mFireCoolDown = fireCoolDown;
            mHullDamage = hullDamage;
            mShielDamage = shieldDamage;
            mParticleColor = particleColor;
            Range = range;
        }

        public void PlaceTurret(Turret turret) => mTurrets.Add(turret);

        public SpaceShip AimingShip => mAimingShip;
        public void AimShip(SpaceShip spaceShip) => mAimingShip = spaceShip;
        public void AimPosition(Vector2? position) => mAimingPos = position;
        public void Fire() => mFire = true;
        public void StopFire() => mFire = false;

        public void Update(GameTime gameTime, SpaceShip origin, GameObjectManager objManager)
        {
            mFireCoolDown += gameTime.ElapsedGameTime.Milliseconds;
            var hasFired = false;
            foreach (var turret in mTurrets)
            {
                var position = mAimingPos ?? CollisionPredictor.PredictPosition(gameTime, origin.Position, 20, mAimingShip);
                turret.GetPosition(origin.Position, origin.Rotation);
                turret.RotateToTArget(origin.Rotation, position);
                if (!mFire || mFireCoolDown < mMaxFireCoolDown) continue;
                turret.Fire(objManager, origin, mParticleColor, mShielDamage, mHullDamage);
                hasFired = true;
            }

            if (hasFired) SoundEffectManager.Instance.PlaySound(SoundEffectRegistries.torpedoFire);
            if (hasFired) mFireCoolDown = 0;
        }

        public void Upgrade(float hullDamagePercentage = 0, float shieldDamagePercentage = 0, float fireCoolDownPercentage = 0)
        {
            mHullDamage += mHullDamage * hullDamagePercentage;
            mShielDamage += mShielDamage * shieldDamagePercentage;
            mFireCoolDown -= mFireCoolDown * fireCoolDownPercentage;
        }

        public void Draw(Scene sceme) { foreach (var weapon in mTurrets) weapon.Draw(sceme); }
    }
}
