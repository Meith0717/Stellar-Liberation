// Items.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.ItemManagement;
using CelestialOdyssey.GameEngine.Content_Management;

namespace CelestialOdyssey.Game.GameObjects
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
