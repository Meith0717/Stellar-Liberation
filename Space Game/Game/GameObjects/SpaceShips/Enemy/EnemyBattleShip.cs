﻿using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.AI.EnemyBehavior;
using CelestialOdyssey.Game.Core;
using Microsoft.Xna.Framework;
using System.Security.Cryptography.X509Certificates;
using CelestialOdyssey.Game.Core.LayerManagement;
using System.Diagnostics;

namespace CelestialOdyssey.Game.GameObjects.SpaceShips.Enemy
{
    public class EnemyBattleShip : Enemy
    {
        public EnemyBattleShip(Vector2 position)
            : base(position, ContentRegistry.enemyBattleShip, 150)
        {
            SensorArray = new(2000000, Configs.SensorArrayCoolDown);

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


            SublightEngine = new(30);

            mAi = new(new()
            {
               new PartolBehavior(),
               new BattleShipAttacBehavior(300000)
            });
        }
        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            sceneManagerLayer.DebugSystem.DrawSensorRadius(Position, 2000000, scene);
        }
    }
}
