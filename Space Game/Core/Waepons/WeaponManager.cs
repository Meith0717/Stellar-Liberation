using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.Content_Management;
using GalaxyExplovive.Core.GameEngine.Position_Management;
using GalaxyExplovive.Core.Waepons;
using GalaxyExplovive.Game.GameObjects.Spacecraft;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GalaxyExplovive.Core.Weapons
{
    public class WeaponManager
    {

        private readonly List<WeaponsProjectile> mProjectiles = new();
        private readonly SoundManager mSoundManager;
        private readonly SpatialHashing<GameEngine.GameObjects.GameObject> mSpatialHashing;

        public WeaponManager(SoundManager soundManager, SpatialHashing<GameEngine.GameObjects.GameObject> spatial)
        {
            mSoundManager = soundManager;
            mSpatialHashing = spatial;
        }

        public void Update(GameTime gameTime)
        {
            int i = 0;
            while (i < mProjectiles.Count)
            {
                if (!mProjectiles[i].Remove)
                {
                    mProjectiles[i].Update(gameTime, mSoundManager, mSpatialHashing);
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