// GameObjectManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.GameObjectManagement
{
    [Serializable]
    public abstract class GameObjectManager
    {
        [JsonProperty] private readonly List<GameObject> mObjects = new();

        protected void AddObj(GameObject obj) => mObjects.Add(obj);

        protected void AddRange(IEnumerable<GameObject> values)
        {
            foreach (GameObject obj in values) AddObj(obj);
        }

        protected virtual bool RemobeObj(GameObject onj) => mObjects.Remove(onj);

        public virtual void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            var lst = new List<GameObject>();

            // Update all obj in Objects
            foreach (var obj in mObjects)
            {
                obj.Update(gameTime, inputState, sceneManagerLayer, scene);

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

        public void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            foreach (var obj in mObjects)
            {
                if (!scene.FrustumCuller.CircleOnWorldView(obj.BoundedBox)) continue;
                obj.Draw(sceneManagerLayer, scene);
            }
        }

    }
}
