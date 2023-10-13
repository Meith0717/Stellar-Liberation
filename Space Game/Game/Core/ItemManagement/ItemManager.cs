// ItemManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.ItemManagement
{
    [Serializable]
    public class ItemManager : GameObjectManager
    {
        public void PopItem(Vector2 momentum, Vector2 position, Item item)
        {
            item.Throw(momentum, position);
            AddObj(item);
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Update(gameTime, inputState, sceneManagerLayer, scene);
        }

        public void CollectItem(Item item) => RemobeObj(item);
    }
}
