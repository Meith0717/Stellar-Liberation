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
        private class Iron : Item { public Iron() : base(ItemID.Iron, GameSpriteRegistries.iron, .2f) => DisposeTime = 60000; }
        private class Titanium : Item { public Titanium() : base(ItemID.Titanium, GameSpriteRegistries.titan, .2f) => DisposeTime = 60000; }
        private class Gold : Item { public Gold() : base(ItemID.Gold, GameSpriteRegistries.gold, .2f) => DisposeTime = 60000; }
        private class Platin : Item { public Platin() : base(ItemID.Platin, GameSpriteRegistries.platin, .2f) => DisposeTime = 60000; }
        private class QuantumCrystals : Item { public QuantumCrystals() : base(ItemID.QuantumCrystals, GameSpriteRegistries.quantumCrystals, .2f) => DisposeTime = 60000; }
        private class DarkMatter : Item { public DarkMatter() : base(ItemID.DarkMatter, GameSpriteRegistries.darkMatter, .2f) => DisposeTime = 60000; }

        public static Item Get(ItemID id, Vector2 momentum, Vector2 position)
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
            item.Throw(momentum, position);
            return item;
        }
    }
}
