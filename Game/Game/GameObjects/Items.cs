// Items.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.ItemManagement;
using StellarLiberation.GameEngine.Content_Management;

namespace StellarLiberation.Game.GameObjects
{
    public class Items
    {
        public class Metall : Item
        {
            public Metall() : base(ContentRegistry.metall, 0.075f, true) => DisposeTime = 60000;

            public override void HasCollide() { }
        }
    }
}
