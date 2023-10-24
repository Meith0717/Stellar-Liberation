// ItemManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Layers;
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

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            base.Update(gameTime, inputState, gameLayer, scene);
        }

        public void RemoveItem(Scene scene, Item item)
        {
            item.RemoveFromSpatialHashing(scene);
            RemobeObj(item);
        } 
    }
}
