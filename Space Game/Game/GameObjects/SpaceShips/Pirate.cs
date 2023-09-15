using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.AI.EnemyAi;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    public class Pirate : SpaceShip
    {
        private patrolBehavior patrolBehavior = new();

        public Pirate(Vector2 position) 
            : base(position, ContentRegistry.pirate.Name, 10) { Velocity = 50; }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            base.Update(gameTime, inputState, sceneLayer);
            patrolBehavior.Update(this, sceneLayer);
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            DrawLive();
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
