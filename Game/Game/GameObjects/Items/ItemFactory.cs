// ItemFactory.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.


// Items.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;

namespace StellarLiberation.Game.GameObjects.Items
{
    public enum ItemID { Metall }

    public static class ItemFactory
    {
        private class Metall : Item { public Metall() : base(TextureRegistries.metall, 0.075f, true, ItemID.Metall) => DisposeTime = 60000; }

        public static Item Get(ItemID id, Vector2 momentum, Vector2 position)
        {
            var item = id switch
            {
                ItemID.Metall => new Metall(),
                _ => throw new System.NotImplementedException()
            };
            item.Throw(momentum, position);
            return item;
        }
    }
}
