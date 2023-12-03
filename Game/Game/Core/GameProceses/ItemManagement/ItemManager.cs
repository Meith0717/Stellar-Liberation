// ItemManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.GameObjects.Items;
using System;

namespace StellarLiberation.Game.Core.GameProceses.ItemManagement
{
    [Serializable]
    public class ItemManager : GameObjectManager
    {
        public void Add(Item item)
        {
            AddObj(item);
        }

        public void RemoveItem(Scene scene, Item item)
        {
            item.RemoveFromSpatialHashing(scene);
            RemobeObj(item);
        }
    }
}
