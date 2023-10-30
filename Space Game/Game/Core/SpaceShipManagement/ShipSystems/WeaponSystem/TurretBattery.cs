// WeaponSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem
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
            foreach (var turret in mTurrets) 
            {
                turret.GetPosition(origin.Position, origin.Rotation);
                turret.RotateToTArget(origin.Rotation, aimingShip?.Position);
                if (!mFire || mFireCoolDown < mMaxFireCoolDown) continue;
                turret.Fire(projectileManager, origin, mParticleColor, mShielDamage, mHullDamage);
            }
            
            if (mFire) SoundManager.Instance.PlaySound(ContentRegistry.torpedoFire, 1f);
        }

        public void Draw(SceneManagerLayer sceneManagerLayer, Scene sceme) { foreach (var weapon in mTurrets) weapon.Draw(sceneManagerLayer, sceme); }
    }
}
