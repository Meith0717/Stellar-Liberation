// OrderBackBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    internal class OrderBackBehavior : Behavior
    {
        private readonly Battleship mBattleship;

        internal OrderBackBehavior(Battleship battleship) => mBattleship = battleship;

        public override double GetScore() =>  mBattleship.Flagship is null ? 0 : double.PositiveInfinity;

        public override void Execute()
        {
            var flagship = mBattleship.Flagship;
            mBattleship.ImpulseDrive.FollowSpaceship(flagship);
            flagship.Hangar.HasFreeSlot();
            if (!flagship.BoundedBox.Intersects(mBattleship.BoundedBox)) return;
            flagship.Hangar.Add(mBattleship);
        }

        public override void Recet()
        {
            mBattleship.Flagship = null;
        }
    }
}
