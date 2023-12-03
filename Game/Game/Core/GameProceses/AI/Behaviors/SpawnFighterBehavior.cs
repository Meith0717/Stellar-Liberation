// SpawnFighterBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;

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
            if (mSpawnCoolDown > mMaxSpawnCoolDown && !(spaceShip.SensorArray.AimingShip is null)) return double.PositiveInfinity;
            return 0;
        }

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, Scene scene)
        {
            for (int i = 0; i < 10; i++)
            {
                var position = ExtendetRandom.NextVectorOnBorder(spaceShip.BoundedBox);
                scene.GameLayer.CurrentSystem.SpaceShipManager.Spawn(position, EnemyId.EnemyFighter);
                mSpawnCoolDown = 0;
            }
        }


        public override void Reset(SpaceShip spaceShip) {; }
    }
}
