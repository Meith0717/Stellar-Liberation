// GameObjectManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    [Serializable]
    public class GameObject2DManager
    {
        [JsonProperty] public readonly GameObject2DTypeList GameObjects2Ds = new();
        [JsonIgnore] private readonly GameLayer mGameLayer;
        [JsonIgnore] private readonly SpatialHashing mSpatialHashing;

        public GameObject2DManager(GameLayer gameLayer, SpatialHashing spatialHashing)
        {
            mGameLayer = gameLayer;
            mSpatialHashing = spatialHashing;
        }

        public GameObject2DManager(GameObject2DTypeList gameObject2Ds, GameLayer gameLayer, SpatialHashing spatialHashing)
        {
            GameObjects2Ds = gameObject2Ds;
            mGameLayer = gameLayer;
            mSpatialHashing = spatialHashing;
            foreach (var obj in gameObject2Ds) obj.Initialize(mGameLayer);
        }

        public void SpawnGameObject2D(GameObject2D obj, bool addToSpatialHash = true)
        {
            GameObjects2Ds.Add(obj);
            obj.Initialize(mGameLayer, addToSpatialHash);
        }

        public bool DespawnGameObject(GameObject2D obj)
        {
            if (!GameObjects2Ds.Remove(obj)) return false;
            mSpatialHashing.RemoveObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
            return true;
        }

        public void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer)
        {
            var copyList = new List<GameObject2D>(GameObjects2Ds);
            foreach (var obj in copyList)
            {
                obj.Update(gameTime, inputState, gameLayer);

                if (!obj.Dispose) continue;
                DespawnGameObject(obj);
            }
        }

        public void Draw(GameLayer scene)
        {
            foreach (var obj in GameObjects2Ds)
            {
                if (!scene.Camera2D.Bounds.Intersects(obj.BoundedBox)) continue;
                obj.Draw(scene);
            }
        }
    }
}
