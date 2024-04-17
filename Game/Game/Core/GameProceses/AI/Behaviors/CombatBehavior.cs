// CombatBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    internal class CombatBehavior : Behavior
    {
        private readonly Battleship mSpaceship;
        private bool mReposition;

        public CombatBehavior(Battleship spaceShip) => mSpaceship = spaceShip;

        public override double GetScore()
        {
            if (mSpaceship.Sensors.Opponents.Count <= 0) return 0;
            var shielHhullScore = mSpaceship.Defense.ShieldPercentage * 0.5 + mSpaceship.Defense.HullPercentage * 0.5;
            var score = shielHhullScore;
            return score;
        }

        public override void Execute()
        {
            var target = mSpaceship.Sensors.Opponents.FirstOrDefault(defaultValue: null);
            if (target is null) return;
            mSpaceship.Weapons.AimTarget(target);
            var distance = Vector2.Distance(target.Position, mSpaceship.Position);

            switch (mReposition)
            {
                case false:
                    mSpaceship.ImpulseDrive.MoveInDirection(Vector2.Normalize(target.Position - mSpaceship.Position));
                    if (distance <= mSpaceship.BoundedBox.Diameter * 5) mReposition = true;
                    break;
                case true:
                    mSpaceship.ImpulseDrive.MoveInDirection(-Vector2.Normalize(target.Position - mSpaceship.Position));
                    if (distance >= mSpaceship.BoundedBox.Diameter * 30) mReposition = false;
                    break;
            }
        }

        public override void Recet() { mSpaceship.ImpulseDrive.Stop(); }
    }
}
