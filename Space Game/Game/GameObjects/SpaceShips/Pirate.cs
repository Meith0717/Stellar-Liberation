using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    public class Pirate : SpaceShip
    {
        public Pirate(Vector2 position) 
            : base(position, ContentRegistry.pirate.Name, 1) { }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            DrawLive();
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
