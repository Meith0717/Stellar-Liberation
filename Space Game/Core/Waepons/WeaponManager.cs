using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Galaxy_Explovive.Core.Waepons;
using Galaxy_Explovive.Game.GameObjects.Spacecraft;

namespace Galaxy_Explovive.Core.Weapons
{
    public class WeaponManager
    {

        private List<WeaponsProjectile> mProjectiles = new();

        public void Update(GameTime gameTime)
        {
            int i = 0;
            while (i < mProjectiles.Count)
            {
                if (!mProjectiles[i].Remove)
                {
                    mProjectiles[i].Update(gameTime);
                    i++;
                    continue;
                }
                mProjectiles.RemoveAt(i);
            }
        }

        public void Draw()
        {
            foreach (WeaponsProjectile projectile in mProjectiles)
            {
                projectile.Draw();
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