// WeaponSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem
{
    public class WeaponSystem
    {
        private readonly List<Weapon> mWeapons = new();
        private readonly Color mWeaponColor;

        public SpaceShip Target;
        public Vector2? TargetPosition;

        private int mShieldDamage;
        private int mHullDamage;

        // Cooldown Stuff
        private int mMaxCoolDown;
        private int mCooldown;

        public WeaponSystem(Color color, int shieldDamage, int hullDamage, int coolDown)
        {
            mWeaponColor = color;
            mShieldDamage = shieldDamage;
            mHullDamage = hullDamage;
            mMaxCoolDown = coolDown;
        }

        public void SetWeapon(Vector2 position) =>
            mWeapons.Add(new(position, 1f, 100, mWeaponColor, mShieldDamage, mHullDamage));

        public virtual void Fire(ProjectileManager projectileManager, SpaceShip spaceShip)
        {
            if (mCooldown < mMaxCoolDown) return;
            mCooldown = 0;
            bool fire = false;

            foreach (var weapon in mWeapons)
            {
                if (!weapon.Fire(spaceShip, out var projectile)) continue;
                projectileManager.AddProjectiel(projectile);
                fire = true;
            }

            if (fire) SoundManager.Instance.PlaySound(ContentRegistry.torpedoFire, 1f);
        }

        public void Update(GameTime gameTime, InputState inputState, SpaceShip origin, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            mCooldown += gameTime.ElapsedGameTime.Milliseconds;

            foreach (var weapon in mWeapons)
            {
                switch (Target)
                {
                    case null:
                        weapon.ForgetTarget();
                        break;
                    case not null:
                        weapon.SetTarget(Target.Position);
                        break;
                }

                if (TargetPosition is not null)
                {
                    weapon.SetTarget((Vector2)TargetPosition);
                }
                weapon.Update(gameTime, inputState, sceneManagerLayer, scene, origin.Rotation, origin.Position);
            }
        }

        public void Draw(SceneManagerLayer sceneManagerLayer, Scene sceme)
        {
            foreach (var weapon in mWeapons)
            {
                weapon.Draw(sceneManagerLayer, sceme);
            }
        }

    }
}
