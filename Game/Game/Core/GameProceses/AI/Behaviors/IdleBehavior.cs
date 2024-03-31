// IdleBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.GameProceses.SpaceshipManagement.Components.PropulsionSystem;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    internal class IdleBehavior : Behavior
    {
        private readonly SublightDrive mSublightDrive;

        public IdleBehavior(SublightDrive sublightDrive) => mSublightDrive = sublightDrive;

        public override void Execute() {; }

        public override double GetScore() => 0.01f;

        public override void Recet() { mSublightDrive.SetVelocity(0); }
    }
}
