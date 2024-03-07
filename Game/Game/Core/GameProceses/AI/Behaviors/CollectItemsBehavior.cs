// CollectItemsBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.GameObjects.Recources;
using StellarLiberation.Game.GameObjects.Recources.Items;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class CollectItemsBehavior : Behavior
    {
        private readonly Spaceship mSpaceship;
        private List<Container> mContainer = new();

        public CollectItemsBehavior(Spaceship spaceShip) => mSpaceship = spaceShip;

        public override double GetScore()
        {
            mContainer = mSpaceship.SensorSystem.ShortRangeScan.OfType<Container>().ToList();
            if (mContainer.Count <= 0) return 0;

            var opponents = mSpaceship.SensorSystem.OpponentsInRannge;
            var opponentsInRageScore = 1 / (1d * opponents.Count + 1);
            if (opponentsInRageScore == 1) return 1;

            var shielHhullScore = mSpaceship.DefenseSystem.ShieldPercentage * .8 + mSpaceship.DefenseSystem.HullPercentage * .2;

            var distanceToFirstOpponent = 1 - ( 1 / (1d * Vector2.Distance(mSpaceship.Position, opponents.First().Position) + 1));
            return opponentsInRageScore * distanceToFirstOpponent * shielHhullScore;    
        }

        public override void Execute()
        {
            if (mContainer.Count <= 0) return;
            mSpaceship.SublightDrive.MoveInDirection(Vector2.Normalize(mContainer.First().Position - mSpaceship.Position));
            mSpaceship.SublightDrive.SetVelocity(1 - 1 / (.001f * Vector2.Distance(mContainer.First().Position, mSpaceship.Position) + 1));
        }

        public override void Recet()
        {
            mContainer.Clear();
        }
    }
}
