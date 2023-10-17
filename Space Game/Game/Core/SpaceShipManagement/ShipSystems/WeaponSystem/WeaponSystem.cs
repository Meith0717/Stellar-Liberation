// WeaponSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.WeaponSystem
{
    public class WeaponSystem
    {
        private readonly List<Weapon> mWeapons = new();
        private readonly Color mWeaponColor;

        public SpaceShip Target;

        private int mShieldDamage;
        private int mHullDamage;
        private bool mFire;
        private int mFiringCount;

        // Cooldown Stuff
        private readonly int mMaxFireCoolDown;
        private readonly int mMaxChargeCoolDown;
        private float mFireCoolDown;
        private float mChargeCoolDown;

        public WeaponSystem(Color color, int shieldDamage, int hullDamage, int fireCoolDown, int chargeCoolDown)
        {
            mWeaponColor = color;
            mShieldDamage = shieldDamage;
            mHullDamage = hullDamage;
            mMaxFireCoolDown = fireCoolDown;
            mMaxChargeCoolDown = chargeCoolDown;
        }

        public void SetWeapon(Vector2 position) =>
            mWeapons.Add(new(position, 1f, 100, mWeaponColor, mShieldDamage, mHullDamage));

        public virtual void Fire() => mFire = true;

        public void Update(GameTime gameTime, InputState inputState, SpaceShip origin, SceneManagerLayer sceneManagerLayer, Scene scene, ProjectileManager projectileManager)
        {
            mChargeCoolDown += gameTime.ElapsedGameTime.Milliseconds;
            mFireCoolDown += gameTime.ElapsedGameTime.Milliseconds;

            bool hasFired = false;
            foreach (var weapon in mWeapons)
            {
                switch (Target)
                {
                    case null:
                        weapon.Update(gameTime, inputState, sceneManagerLayer, scene, origin.Rotation, origin.Position,  null);
                        break;
                    case not null:
                        weapon.Update(gameTime, inputState, sceneManagerLayer, scene, origin.Rotation, origin.Position, Target.Position);
                        break;
                }

                if (mFire)
                {
                    if (mChargeCoolDown < mMaxChargeCoolDown)
                    {
                        mFire = false;
                        continue;
                    }
                    if (mFireCoolDown < mMaxFireCoolDown) continue;
                    projectileManager.AddProjectiel(weapon.Fire(origin));
                    hasFired = true;
                }
            }

            if (hasFired)
            {
                mFiringCount++;
                mFireCoolDown = 0;
                SoundManager.Instance.PlaySound(ContentRegistry.torpedoFire, 1f);
            }
            

            if (mFiringCount < 20) return;
            mFiringCount = 0;
            mChargeCoolDown = 0;
            mFire = false;
        }

        public void Draw(SceneManagerLayer sceneManagerLayer, Scene sceme) { foreach (var weapon in mWeapons) weapon.Draw(sceneManagerLayer, sceme); }

    }
}
