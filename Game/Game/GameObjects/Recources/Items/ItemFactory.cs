// ItemFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.


// Items.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
namespace StellarLiberation.Game.GameObjects.Recources.Items
{
    public enum ItemID { Iron, Titanium, Gold, Platin, QuantumCrystals, DarkMatter, }

    public static class ItemFactory
    {
        private class Iron : Item { public Iron() : base(ItemID.Iron, GameSpriteRegistries.iron) { } }
        private class Titanium : Item { public Titanium() : base(ItemID.Titanium, GameSpriteRegistries.titan) { } }
        private class Gold : Item { public Gold() : base(ItemID.Gold, GameSpriteRegistries.gold) { } }
        private class Platin : Item { public Platin() : base(ItemID.Platin, GameSpriteRegistries.platin) { } }
        private class QuantumCrystals : Item { public QuantumCrystals() : base(ItemID.QuantumCrystals, GameSpriteRegistries.quantumCrystals) { } }
        private class DarkMatter : Item { public DarkMatter() : base(ItemID.DarkMatter, GameSpriteRegistries.darkMatter) { } }

        public static Item Get(ItemID id)
        {
            Item item = id switch
            {
                ItemID.Iron => new Iron(),
                ItemID.Titanium => new Titanium(),
                ItemID.Gold => new Gold(),
                ItemID.Platin => new Platin(),
                ItemID.QuantumCrystals => new QuantumCrystals(),
                ItemID.DarkMatter => new DarkMatter(),
                _ => throw new System.NotImplementedException()
            };
            return item;
        }
    }
}
