using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialOdyssey.Game.Core.ShipSystems.WeaponSystem
{
    public class ProjectileManager
    {
        public List<Projectile> Projectiles { get; private set; } = new();

        public void AddProjectiel(Projectile projectile)
        {
            Projectiles.Add(projectile);
        }

        public void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            List<Projectile> deleteList = new();
            foreach (var projectile in Projectiles)
            {
                if (projectile.LiveTime <= 0)
                {
                    deleteList.Add(projectile);
                    continue;
                }
                projectile.Update(gameTime, inputState, sceneManagerLayer, scene);
            }

            foreach (var projectile in deleteList)
            {
                projectile.RemoveFromSpatialHashing(scene);
                Projectiles.Remove(projectile);
            }

        }
    }
}
