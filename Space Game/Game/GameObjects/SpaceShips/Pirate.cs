using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    public class Pirate : SpaceShip
    {
        public Pirate(Vector2 position) 
            : base(position, ContentRegistry.pirate.Name, 1) { }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            DrawLive();
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
