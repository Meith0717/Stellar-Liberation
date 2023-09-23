using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.AI;
using CelestialOdyssey.Game.Core.AI.EnemyBehavior;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips
{
    public class Enemy : SpaceShip
    {
        private BehaviorBasedAI mAi;

        public Enemy(Vector2 position) 
            : base(position, ContentRegistry.pirate.Name, 10) 
        {
            WeaponSystem = new(new(){ new(0, 0) }, 300);
            mAi = new();
            mAi.AddBehavior(new PartolBehavior());
            mAi.AddBehavior(new FollowBehavior());
            mAi.AddBehavior(new FleeBehavior());
            mAi.AddBehavior(new AttacBehavior(100, 250000));
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            base.Update(gameTime, inputState, sceneLayer);
            mAi.Update(gameTime, SensorArray.SortedObjectsInRange, this);
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            DefenseSystem.DrawLive(this);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
