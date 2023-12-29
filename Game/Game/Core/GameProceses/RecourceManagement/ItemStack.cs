// InventoryItem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.Recources.Items;

namespace StellarLiberation.Game.Core.GameProceses.RecourceManagement
{
    public class ItemStack
    {
        public int Count;
        public readonly ItemID ItemId;
        public readonly string Texture;

        public ItemStack(ItemID itemId, string texture)
        {
            Count = 0; ItemId = itemId; Texture = texture;
        }

        public bool Add(Item item) 
        {
            if (item.ItemID != ItemId) return false;
            item.Dispose = true;
            Count++;
            return true;
        }

        public bool Pop(Vector2 position)
        {
            if (Count == 0) return false;
            ExtendetRandom.Random.NextUnitVector(out var dir);
            ItemFactory.Get(ItemId, dir, position);
            return true;    
        }

        public void Remove(int amount = 1) => Count-=amount;

        public override string ToString()
        {
            return $"{ItemId}: {Count}";
        }
    }
}
