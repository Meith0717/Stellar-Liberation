using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.AI.EnemyBehavior;
using CelestialOdyssey.Game.Core;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips.Enemy
{
    public class EnemyBattleShip : Enemy
    {
        public EnemyBattleShip(Vector2 position)
            : base(position, ContentRegistry.enemyBattleShip, 20)
        {
            SensorArray = new(2000000, 1000);

            WeaponSystem = new(Color.Red, 1, 1, 500);
            WeaponSystem.SetWeapon(new(50000, 20000));
            WeaponSystem.SetWeapon(new(50000, -20000));
            WeaponSystem.SetWeapon(new(20000, 20000));
            WeaponSystem.SetWeapon(new(20000, -20000));
            WeaponSystem.SetWeapon(new(0, 20000));
            WeaponSystem.SetWeapon(new(0, -20000));
            WeaponSystem.SetWeapon(new(-50000, 20000));
            WeaponSystem.SetWeapon(new(-50000, -20000));
            WeaponSystem.SetWeapon(new(-20000, 20000));
            WeaponSystem.SetWeapon(new(-20000, -20000));


            SublightEngine = new(5);

            mAi = new(new()
            {
               new PartolBehavior(),
               new BattleShipAttacBehavior(300000)
            });
        }
    }
}
