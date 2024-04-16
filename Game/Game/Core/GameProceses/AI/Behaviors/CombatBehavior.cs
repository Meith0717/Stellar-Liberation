// CombatBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.Spacecrafts;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class CombatBehavior : Behavior
    {
        private readonly Flagship mSpaceship;
        private bool mReposition;

        public CombatBehavior(Flagship spaceShip) => mSpaceship = spaceShip;

        public override double GetScore()
        {
            return 0;
        }

        public override void Execute()
        {

        }

        public override void Recet() {; }
    }
}
