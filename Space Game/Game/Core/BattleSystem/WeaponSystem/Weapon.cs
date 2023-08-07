using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.BattleSystem.WeaponSystem.Projectiles;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.BattleSystem.WeaponSystem
{
    public class PhotonTorpedo : Weapon
    {
        public PhotonTorpedo() : base(5000, "") { }

        public override void Fire(SpaceShip origin, SpaceShip target)
        {
            var projectile = new Torpedo(origin.Position, target, ContentRegistry.photonTorpedo.Name, 2, 5);
            AddProjectile(projectile);
        }
    }

    public class PhotonPhaser : Weapon
    {
        public PhotonPhaser() : base(2000, "") { }

        public override void Fire(SpaceShip origin, SpaceShip target)
        {
            var projectile = new Phaser(origin, target, Color.LightYellow, 2, 5);
            AddProjectile(projectile);
        }
    }

    public abstract class Weapon
    {
        private List<Projectile> mProjecvtiles = new();
        private int mMaxCoolDown;
        private int mCooldown;
        private string mSound;

        internal Weapon(int coolDownMs, string sound)
        {
            mCooldown = mMaxCoolDown = coolDownMs;
        }

        public abstract void Fire(SpaceShip origin, SpaceShip target);

        internal virtual void AddProjectile(Projectile projectile)
        { 
            if (mCooldown < mMaxCoolDown) return; 
            mProjecvtiles.Add(projectile);
            // SoundManager.Instance.PlaySound()
            mCooldown = 0;
        }

        public void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            List<Projectile> deleteList = new();

            mCooldown += gameTime.ElapsedGameTime.Milliseconds;

            foreach (var weapon in mProjecvtiles)
            {
                if (weapon.LiveTime <= 0)
                {
                    deleteList.Add(weapon);
                    continue;
                }
                weapon.Update(gameTime, inputState, engine);
            }

            foreach (var weapon in deleteList)
            {
                weapon.RemoveFromSpatialHashing(engine);
                mProjecvtiles.Remove(weapon);
            }
        }
    }
}
