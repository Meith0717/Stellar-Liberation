// InventoryItem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.Recources.Items;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    internal class InventoryItem
    {
        public int Count;
        public readonly ItemID ItemId;
        public readonly string Texture;

        internal InventoryItem(ItemID itemId, string texture)
        {
            Count = 1; ItemId = itemId; Texture = texture;
        }

        internal void Add(int amount = 1) => Count+=amount;

        internal void Remove(int amount = 1) => Count-=amount;

        public override string ToString()
        {
            return $"{ItemId}: {Count}";
        }
    }
}
