// ItemManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Layers;
using Microsoft.Xna.Framework;
using System;

namespace StellarLiberation.Game.Core.ItemManagement
{
    [Serializable]
    public class ItemManager : GameObjectManager
    {
        public void PopItem(Vector2 momentum, Vector2 position, Item item)
        {
            item.Throw(momentum, position);
            AddObj(item);
        }

        public void RemoveItem(Scene scene, Item item)
        {
            item.RemoveFromSpatialHashing(scene);
            RemobeObj(item);
        } 
    }
}
