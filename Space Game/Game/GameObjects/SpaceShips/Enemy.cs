using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core;
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
        private readonly BehaviorBasedAI mAi;

        public Enemy(Vector2 position) 
            : base(position, ContentRegistry.pirate.Name, 10) 
        {
            SensorArray = new(Configs.Enemy.ScanDistance, Configs.SensorArrayCoolDown);
            WeaponSystem = new(new(){ new(0, 0) }, 300);
            SublightEngine = new(50);
            mAi = new(new()
            {
               new PartolBehavior(Configs.Enemy.PatrollVelocity),
               new FollowBehavior(Configs.Enemy.FollowVelocity),
               new FleeBehavior(Configs.Enemy.FleeVelovity),
               new AttacBehavior(Configs.Enemy.AttacVelocity, Configs.Enemy.AttackDistance)
            });        
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
