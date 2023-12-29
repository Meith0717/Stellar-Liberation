// GameObjectManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    [Serializable]
    public class GameObjectManager
    {
        [JsonProperty] public readonly List<GameObject2D> Objects = new();
        [JsonProperty] protected readonly List<GameObject2D> mAddedObjects = new();
        [JsonProperty] protected readonly List<GameObject2D> mRemovedObjects = new();

        public void AddObj(GameObject2D obj) => mAddedObjects.Add(obj);

        public void AddRange(IEnumerable<GameObject2D> values)
        {
            foreach (GameObject2D obj in values) AddObj(obj);
        }

        public virtual bool RemobeObj(GameObject2D onj)
        {
            if (!Objects.Contains(onj)) return false;
            mRemovedObjects.Add(onj);
            return true;
        }

        public virtual void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            Objects.AddRange(mAddedObjects);
            mAddedObjects.Clear();

            // Update all obj in Objects
            foreach (var obj in Objects)
            {
                obj.Update(gameTime, inputState, scene);

                // Check if obj is Disposed
                if (obj.Dispose) mRemovedObjects.Add(obj);
            }

            // Delete Disposed obj from Objects list
            foreach (var item in mRemovedObjects)
            {
                Objects.Remove(item);
                item.RemoveFromSpatialHashing(scene);
            }
            mRemovedObjects.Clear();
        }
    }
}
