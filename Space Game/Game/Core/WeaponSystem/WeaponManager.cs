using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.Game.GameObjects.Weapons;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.WeaponSystem
{
    public class WeaponManager
    {
        private List<Weapon> mWeapons = new();

        public void AddTorpedo(SpaceShip ship, GameObject targetObj, ProjectileType type = ProjectileType.None)
        {
            mWeapons.Add(new Torpedo(ship, targetObj, ship.Rotation - MathF.PI, type));
        }

        public void AddLaser(SpaceShip ship, GameObject targetObj)
        {
            mWeapons.Add(new Laser(ship, targetObj));
        }

        public void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            List<Weapon> deleteList = new();

            foreach (var weapon in mWeapons)
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
                mWeapons.Remove(weapon);
            }
        }

    }
}
