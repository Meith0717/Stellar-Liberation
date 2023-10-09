using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.AI.EnemyBehavior;
using CelestialOdyssey.Game.Core;
using Microsoft.Xna.Framework;
using CelestialOdyssey.Game.Core.LayerManagement;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips.Enemy
{
    public class EnemyFighter : Enemy
    {
        public EnemyFighter(Vector2 position)
            : base(position, ContentRegistry.enemyFighter, 20)
        {
            SensorArray = new(2000000, Configs.SensorArrayCoolDown);

            WeaponSystem = new(Color.Red, 1, 1, 500);
            WeaponSystem.SetWeapon(new(0, 50));

            SublightEngine = new(50);

            mAi = new(new()
            {
               new PartolBehavior(),
               new FollowBehavior(),
               new FleeBehavior(2500000),
               new FighterAttacBehavior(250000)
            });
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            sceneManagerLayer.DebugSystem.DrawSensorRadius(Position, 2000000, scene);
        }
    }
}
