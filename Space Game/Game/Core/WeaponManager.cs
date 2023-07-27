using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.Game.GameObjects.Weapons;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core
{
    public class WeaponManager
    {
        private List<Projectile> projectiles = new();

        public void AddProjectile(SpaceShip ship, GameObject targetObj, ProjectileType type = ProjectileType.None)
        {
            var position = Geometry.GetPointOnCircle(ship.Position, ship.BoundedBox.Radius + 10, ship.Rotation - MathF.PI / 2);
            projectiles.Add(new(position, targetObj, ship.Rotation - MathF.PI / 2, type));
        }

        public void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            List<Projectile> deleteList = new();
            foreach (var projectile in projectiles)
            {
                if (projectile.LiveTime > 10)
                {
                    deleteList.Add(projectile);
                    continue;
                }
                projectile.Update(gameTime, inputState, engine);
            }
            foreach (var item in deleteList)
            {
                item.RemoveFromSpatialHashing(engine);
                projectiles.Remove(item);
            }
        }

    }
}
