// SpawnFighterBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    internal class SpawnFighterBehavior : Behavior
    {

        private readonly int mMaxSpawnCoolDown;
        private int mSpawnCoolDown;

        public SpawnFighterBehavior(int spawnCooldown) => mMaxSpawnCoolDown = mSpawnCoolDown = spawnCooldown;

        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            mSpawnCoolDown += gameTime.ElapsedGameTime.Milliseconds;
            if (mSpawnCoolDown > mMaxSpawnCoolDown && spaceShip.SensorSystem.OpponentsInRannge.Any()) return double.PositiveInfinity;
            return 0;
        }

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            for (int i = 0; i < 15; i++)
            {
                var position = ExtendetRandom.NextVectorOnBorder(spaceShip.BoundedBox);
                var fighter = SpaceShipFactory.Get(position, ShipID.Bomber, Fractions.Enemys);
                var target = ExtendetRandom.GetRandomElement(spaceShip.SensorSystem.OpponentsInRannge);
                fighter.WeaponSystem.AimShip(target);
                scene.GameLayer.CurrentSystem.GameObjectManager.AddObj(fighter);
                mSpawnCoolDown = 0;
            }
        }


        public override void Reset(SpaceShip spaceShip) {; }
    }
}
