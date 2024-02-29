// IdleBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PropulsionSystem;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    internal class IdleBehavior : Behavior
    {
        private readonly SublightDrive mSublightDrive;

        public IdleBehavior(SublightDrive sublightDrive) => mSublightDrive = sublightDrive;

        public override void Execute() => mSublightDrive.SetVelocity(0);

        public override double GetScore() => 0.01f;

        public override void Recet() {; }
    }
}
