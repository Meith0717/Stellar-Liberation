// SpawnFighterBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
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

        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            mSpawnCoolDown += gameTime.ElapsedGameTime.Milliseconds;
            if (mSpawnCoolDown > mMaxSpawnCoolDown && spaceShip.SensorSystem.OpponentsInRannge.Any()) return double.PositiveInfinity;
            return 0;
        }

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            for (int i = 0; i < 15; i++)
            {
                var position = ExtendetRandom.NextVectorOnBorder(spaceShip.BoundedBox);
                SpaceShipFactory.Spawn(spaceShip.PlanetSystem, position, ShipID.Bomber, Fractions.Enemys, out var fighter);
                var target = ExtendetRandom.GetRandomElement(spaceShip.SensorSystem.OpponentsInRannge);
                fighter.WeaponSystem.AimShip(target);
                mSpawnCoolDown = 0;
            }
        }


        public override void Reset(SpaceShip spaceShip) {; }
    }
}
