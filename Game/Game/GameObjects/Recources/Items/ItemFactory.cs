// ItemFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.


// Items.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;

namespace StellarLiberation.Game.GameObjects.Recources.Items
{
    public enum ItemID { Iron, Titanium, Gold, Platin, QuantumCrystals, DarkMatter, }

    public static class ItemFactory
    {
        private class Iron : Item { public Iron() : base(GameSpriteRegistries.metall, 0.075f, true, ItemID.Iron) => DisposeTime = 60000; }
        private class Titanium : Item { public Titanium() : base(GameSpriteRegistries.metall, 0.075f, true, ItemID.Titanium) => DisposeTime = 60000; }
        private class Gold : Item { public Gold() : base(GameSpriteRegistries.metall, 0.075f, true, ItemID.Gold) => DisposeTime = 60000; }
        private class Platin : Item { public Platin() : base(GameSpriteRegistries.metall, 0.075f, true, ItemID.Platin) => DisposeTime = 60000; }
        private class QuantumCrystals : Item { public QuantumCrystals() : base(GameSpriteRegistries.metall, 0.075f, true, ItemID.QuantumCrystals) => DisposeTime = 60000; }
        private class DarkMatter : Item { public DarkMatter() : base(GameSpriteRegistries.metall, 0.075f, true, ItemID.DarkMatter) => DisposeTime = 60000; }

        public static Item Get(ItemID id, Vector2 momentum, Vector2 position)
        {
            var item = id switch
            {
                ItemID.Iron => new Iron(),
                _ => throw new System.NotImplementedException()
            };
            item.Throw(momentum, position);
            return item;
        }
    }
}
