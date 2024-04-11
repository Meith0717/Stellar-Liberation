// IdleBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    internal class IdleBehavior : Behavior
    {
        private readonly ImpulseDrive mSublightDrive;

        public IdleBehavior(ImpulseDrive sublightDrive) => mSublightDrive = sublightDrive;

        public override void Execute() {; }

        public override double GetScore() => 0.01f;

        public override void Recet() {; }
    }
}
