// CombatBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class CombatBehavior : Behavior
    {
        private readonly Flagship mSpaceship;
        private bool mReposition;
        private double mBias1;

        public CombatBehavior(Flagship spaceShip)
        {
            mSpaceship = spaceShip;
            mBias1 = .7f + ExtendetRandom.Random.NextSingle() * .2f;
        }

        public override double GetScore()
        {
            return 0;
        }

        public override void Execute()
        {

        }

        public override void Recet()
        {
            mBias1 = .4f + ExtendetRandom.Random.NextSingle() * .2f;
        }
    }
}
