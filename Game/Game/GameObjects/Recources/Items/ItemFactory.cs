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
        private class Iron : Item { public Iron() : base(GameSpriteRegistries.iron, .2f, true, ItemID.Iron) => DisposeTime = 60000; }
        private class Titanium : Item { public Titanium() : base(GameSpriteRegistries.titan, .2f, true, ItemID.Titanium) => DisposeTime = 60000; }
        private class Gold : Item { public Gold() : base(GameSpriteRegistries.gold, .2f, true, ItemID.Gold) => DisposeTime = 60000; }
        private class Platin : Item { public Platin() : base(GameSpriteRegistries.platin, .2f, true, ItemID.Platin) => DisposeTime = 60000; }
        private class QuantumCrystals : Item { public QuantumCrystals() : base(GameSpriteRegistries.quantumCrystals, .2f, true, ItemID.QuantumCrystals) => DisposeTime = 60000; }
        private class DarkMatter : Item { public DarkMatter() : base(GameSpriteRegistries.darkMatter, .2f, true, ItemID.DarkMatter) => DisposeTime = 60000; }

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
