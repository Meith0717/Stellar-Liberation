// TurretBattery.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.Collision_Detection;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.LayerManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem
{
    public class TurretBattery
    {
        private readonly List<Turret> mTurrets = new();
        private readonly int mMaxFireCoolDown;
        private float mFireCoolDown;
        private bool mFire;
        private Color mParticleColor;
        private int mHullDamage;
        private int mShielDamage;

        public TurretBattery(int fireCoolDown, Color particleColor, int hullDamage, int shieldDamage)
        {
            mMaxFireCoolDown = fireCoolDown;
            mFireCoolDown = fireCoolDown;
            mHullDamage = hullDamage;
            mShielDamage = shieldDamage;
            mParticleColor = particleColor;
        }
 
        public void PlaceTurret(Turret turret) => mTurrets.Add(turret);

        public void Fire() => mFire = true;

        public void StopFire() => mFire = false;

        public void Update(GameTime gameTime, SpaceShip origin, ProjectileManager projectileManager, SpaceShip aimingShip)
        {
            mFireCoolDown += gameTime.ElapsedGameTime.Milliseconds;
            var hasFired = false;
            foreach (var turret in mTurrets) 
            {
                var position = CollisionPredictor.PredictPosition(gameTime, origin.Position, 20, aimingShip);
                turret.GetPosition(origin.Position, origin.Rotation);
                turret.RotateToTArget(origin.Rotation, position);
                if (!mFire || mFireCoolDown < mMaxFireCoolDown) continue;
                turret.Fire(projectileManager, origin, mParticleColor, mShielDamage, mHullDamage);
                hasFired = true;
            }
            
            if (hasFired) SoundManager.Instance.PlaySound(SoundRegistries.torpedoFire, 1f);
            if (hasFired) mFireCoolDown = 0;
        }

        public void Draw(Scene sceme) { foreach (var weapon in mTurrets) weapon.Draw(sceme); }
    }
}
