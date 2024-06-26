// IDELBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.


using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    internal class IDELBehavior : Behavior
    {
        private readonly Battleship mBattleship;
        private Vector2? mPatrolTarget;

        internal IDELBehavior(Battleship spacecraft) => mBattleship = spacecraft;

        public override double GetScore()
        {
            var shielHhullScore = mBattleship.Defense.ShieldPercentage * 0.5 + mBattleship.Defense.HullPercentage * 0.5;
            var hasNoAimingShip = !mBattleship.Sensors.Opponents.Any() ? 1 : 0;

            var score = shielHhullScore * hasNoAimingShip;
            return score;
        }

        public override void Execute()
        {

            switch (mPatrolTarget)
            {
                case null:
                    // Get Planets in Radius
                    var patrolTargets = mBattleship.Sensors.PlanetsystemState.Planets;
                    if (!patrolTargets.Any()) break;

                    // Get Random Target
                    var randomPlanet = ExtendetRandom.GetRandomElement(patrolTargets);
                    var angleBetweenTargetAndShip = Geometry.AngleBetweenVectors(randomPlanet.Position, mBattleship.Position);
                    var target = Geometry.GetPointOnCircle(randomPlanet.BoundedBox, angleBetweenTargetAndShip);
                    mPatrolTarget = target;

                    // Send Ship to Target
                    mBattleship.ImpulseDrive.MoveToTarget(target);
                    break;
                case not null:
                    // Check if Target is Reached
                    if (Vector2.Distance((Vector2)mPatrolTarget, mBattleship.Position) < 1000) mPatrolTarget = null;
                    break;
            }
        }

        public override void Recet() {; }
    }
}
