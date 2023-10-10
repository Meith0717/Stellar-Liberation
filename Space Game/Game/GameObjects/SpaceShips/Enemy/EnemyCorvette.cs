using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.AI.EnemyBehavior;
using CelestialOdyssey.Game.Core;
using Microsoft.Xna.Framework;
using CelestialOdyssey.Game.Core.LayerManagement;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips.Enemy
{
    public class EnemyCorvette : Enemy
    {
        public EnemyCorvette(Vector2 position)
            : base(position, ContentRegistry.enemyCorvette, 5f)
        {
            SensorArray = new(2000000, 1000);

            WeaponSystem = new(Color.Red, 1, 1, 500);
            WeaponSystem.SetWeapon(new(0, 0));

            SublightEngine = new(5);

            mAi = new(new()
            {
               new PartolBehavior(),
               new FollowBehavior(),
               new FighterAttacBehavior(250000)
            }) ;
        }
    }
}
