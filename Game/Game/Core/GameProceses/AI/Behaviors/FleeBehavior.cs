// FleeBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.GameObjects.SpaceCrafts;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class FleeBehavior : Behavior
    {
        private readonly Flagship mSpaceship;

        public FleeBehavior(Flagship spaceShip)
            => mSpaceship = spaceShip;

        public override double GetScore()
        {
            return 0;
        }

        public override void Execute()
        {
        }

        public override void Recet()
        {
        }
    }
}
