// CollectItemsBehavior.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.GameObjects.Recources.Items;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class CollectItemsBehavior : Behavior
    {
        private readonly SpaceShip mSpaceShip;
        private List<Item> mItems = new();

        public CollectItemsBehavior(SpaceShip spaceShip) => mSpaceShip = spaceShip;

        public override double GetScore()
        {
            mItems = mSpaceShip.SensorSystem.ShortRangeScan.OfType<Item>()
                .Where((item) => mSpaceShip.Inventory.HasSpace(item.ItemID)).ToList();
            if (mItems.Count > 0) return 1;
            return 0;
        }

        public override void Execute()
        {
            if (mItems.Count <= 0) return;
            mSpaceShip.SublightDrive.MoveInDirection(Vector2.Normalize(mItems.First().Position - mSpaceShip.Position));
            mSpaceShip.SublightDrive.SetVelocity(.3f);
        }


        public override void Recet()
        {
            mItems.Clear();
        }
    }
}
