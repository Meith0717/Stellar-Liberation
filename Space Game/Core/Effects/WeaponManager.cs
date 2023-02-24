using Microsoft.Xna.Framework;
using Space_Game.Core.GameObject;
using Space_Game.Game.GameObjects;
using System.Collections.Generic;
using System.Diagnostics;

namespace Space_Game.Core.Effects
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

        public void Shoot(Ship originShip, Ship targetShip, Color color, float maxDistance)
        {
            mProjectiles.Add(new WeaponsProjectile(originShip, targetShip, color, maxDistance));
        }

        public void RemoveProjectile(WeaponsProjectile projectile)
        {
            mProjectiles.Remove(projectile);
        }

    }
}
