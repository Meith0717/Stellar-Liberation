// GameObjectManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameObjectManagement
{
    [Serializable]
    public abstract class GameObjectManager
    {
        [JsonProperty] protected readonly List<GameObject2D> mObjects = new();

        protected void AddObj(GameObject2D obj) => mObjects.Add(obj);

        protected void AddRange(IEnumerable<GameObject2D> values)
        {
            foreach (GameObject2D obj in values) AddObj(obj);
        }

        protected virtual bool RemobeObj(GameObject2D onj) => mObjects.Remove(onj);

        public virtual void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            var lst = new List<GameObject2D>();

            // Update all obj in Objects
            foreach (var obj in mObjects)
            {
                obj.Update(gameTime, inputState, scene);

                // Check if obj is Disposed
                if (obj.Dispose) lst.Add(obj);
            }

            // Delete Disposed obj from Objects list
            foreach (var item in lst)
            {
                item.TextureColor = Color.Multiply(item.TextureColor, 0.9f);
                if (item.TextureColor.A > 50) continue;
                RemobeObj(item);
                item.RemoveFromSpatialHashing(scene);
            }
        }

        public void Draw(Scene scene)
        {
            foreach (var obj in mObjects)
            {
                if (!scene.ViewFrustumFilter.CircleOnWorldView(obj.BoundedBox)) continue;
                obj.Draw(scene);
            }
        }

    }
}
