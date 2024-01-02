// ItemStack.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.Recources.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    public class ItemStack
    {
        public int Amount { get; private set; } = 1;
        public readonly ItemID ItemID;

        public ItemStack(ItemID itemID) => ItemID = itemID;

        public void Add(int amount = 1) => Amount+=amount;

        public void Remove(int amount = 1) => Amount-=amount;
    }
}
