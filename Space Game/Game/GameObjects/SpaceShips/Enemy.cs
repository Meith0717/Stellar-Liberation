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
            : base(position, ContentRegistry.enemyCorvette, 30) 
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

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            mAi.Update(gameTime, SensorArray.SortedObjectsInRange, this);
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            DefenseSystem.DrawLive(this);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
