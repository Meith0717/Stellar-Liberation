// ItemValues.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.Recources.Items;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    public static class ItemValues
    {
        public static readonly Dictionary<ItemID, int> Values = new()
        {
            {ItemID.Iron, 1},
            {ItemID.Titanium, 2},
            {ItemID.Gold, 4},
            {ItemID.Platin, 5},
            {ItemID.QuantumCrystals, 10},
            {ItemID.DarkMatter, 50}
        };

        public static int GetValue(ItemID itemID) => Values[itemID];
    }
}
