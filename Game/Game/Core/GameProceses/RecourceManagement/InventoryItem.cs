// InventoryItem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.GameObjects.Recources.Items;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    public class InventoryItem
    {
        public int Count;
        public readonly ItemID ItemId;
        public readonly string Texture;

        public InventoryItem(ItemID itemId, string texture)
        {
            Count = 1; ItemId = itemId; Texture = texture;
        }

        public void Add(int amount = 1) => Count+=amount;

        public void Remove(int amount = 1) => Count-=amount;

        public override string ToString()
        {
            return $"{ItemId}: {Count}";
        }
    }
}
