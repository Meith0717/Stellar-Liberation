using CelestialOdyssey.Game.Core.AI.EnemyBehavior;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips.Enemy
{
    public class EnemyFighter : Enemy
    {
        public EnemyFighter(Vector2 position)
            : base(position, ContentRegistry.enemyFighter, 0.5f)
        {
            SensorArray = new(10000, 1000);

            WeaponSystem = new(Color.Red, 1, 1, 100);
            WeaponSystem.SetWeapon(new(0, 0));

            SublightEngine = new(1);

            mAi = new(new()
            {
               new PartolBehavior(),
               new FollowBehavior(),
               new FleeBehavior(50000),
               new FighterAttacBehavior(8000)
            });
        }

        public override void HasCollide()
        {
            throw new System.NotImplementedException();
        }
    }
}
