using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem
{
    public class Weapons
    {
        private readonly List<Weapon> mWeapons = new();
        private readonly Color mWeaponColor;

        private int mShieldDamage;
        private int mHullDamage;

        // Cooldown Stuff
        private int mMaxCoolDown;
        private int mCooldown;

        public Weapons(Color color, int shieldDamage, int hullDamage, int coolDown)
        {
            mWeaponColor = color;
            mShieldDamage = shieldDamage;
            mHullDamage = hullDamage;
            mMaxCoolDown = coolDown;
        }

        public void SetWeapon(Vector2 position) => 
            mWeapons.Add(new(position, 20, 100, mWeaponColor, mShieldDamage, mHullDamage));

        public virtual void Fire(ProjectileManager projectileManager, SpaceShip spaceShip, Vector2 target)
        { 
            if (mCooldown < mMaxCoolDown) return;
            mCooldown = 0;

            SoundManager.Instance.PlaySound(ContentRegistry.torpedoFire, 1f);

            foreach (var weapon in mWeapons)
            {
                projectileManager.AddProjectiel(weapon.Fire(spaceShip, target));
            }
        }

        public void Update(GameTime gameTime, InputState inputState, SpaceShip origin, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            mCooldown += gameTime.ElapsedGameTime.Milliseconds;

            foreach (var weapon in mWeapons)
            {
                weapon.Update(gameTime, inputState, sceneManagerLayer, scene, origin.Rotation, origin.Position);
            }
        }

        public void Draw(SceneManagerLayer sceneManagerLayer, Scene sceme)
        {
            foreach(var weapon in mWeapons)
            {
                weapon.Draw(sceneManagerLayer, sceme);
            }
        }

    }
}
