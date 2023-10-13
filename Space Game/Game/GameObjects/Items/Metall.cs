// Metall.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.ItemManagement;
using CelestialOdyssey.GameEngine.Content_Management;

namespace CelestialOdyssey.Game.GameObjects.Items
{
    public class Metall : Item
    {
        public Metall() : base(ContentRegistry.metall, 0.075f, 30) => DisposeTime = 60000;

        public override void HasCollide() { }
    }
}
