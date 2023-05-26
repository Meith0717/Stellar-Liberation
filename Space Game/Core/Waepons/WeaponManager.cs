using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Galaxy_Explovive.Core.Waepons;
using Galaxy_Explovive.Game.GameObjects.Spacecraft;
using Galaxy_Explovive.Core.SoundManagement;
using Galaxy_Explovive.Core.TextureManagement;

namespace Galaxy_Explovive.Core.Weapons
{
    public class WeaponManager
    {

        private readonly List<WeaponsProjectile> mProjectiles = new();
        private readonly SoundManager mSoundManager;

        public WeaponManager(SoundManager soundManager) 
        { 
            mSoundManager = soundManager;
        }

        public void Update(GameTime gameTime)
        {
            int i = 0;
            while (i < mProjectiles.Count)
            {
                if (!mProjectiles[i].Remove)
                {
                    mProjectiles[i].Update(gameTime, mSoundManager);
                    i++;
                    continue;
                }
                mProjectiles.RemoveAt(i);
            }
        }

        public void Draw(TextureManager textureManager)
        {
            foreach (WeaponsProjectile projectile in mProjectiles)
            {
                projectile.Draw(textureManager);
            }

        }

        public void Shoot(Spacecraft originShip, Spacecraft targetShip, Color color, float maxDistance)
        {
            mProjectiles.Add(new WeaponsProjectile(originShip, targetShip, color, maxDistance));
        }

        public void RemoveProjectile(WeaponsProjectile projectile)
        {
            mProjectiles.Remove(projectile);
        }

    }
}