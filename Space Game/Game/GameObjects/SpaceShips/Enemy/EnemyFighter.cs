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
            : base(position, ContentRegistry.enemyFighter, 0.5f)
        {
            SensorArray = new(10000, 1000);

            WeaponSystem = new(Color.Red, 1, 1, 500);
            WeaponSystem.SetWeapon(new(0, 50));

            SublightEngine = new(1);

            mAi = new(new()
            {
               new PartolBehavior(),
               new FollowBehavior(),
               new FleeBehavior(50000),
               new FighterAttacBehavior(8000)
            });
        }
    }
}
