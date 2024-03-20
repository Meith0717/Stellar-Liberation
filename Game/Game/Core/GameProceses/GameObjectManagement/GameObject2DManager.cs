// GameObject2DTypeList.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    [Serializable]
    public class GameObject2DManager
    {
        [JsonProperty] private readonly Dictionary<Type, List<GameObject2D>> mTypeDict = new();
        [JsonProperty] private readonly List<GameObject2D> AllObjects = new();

        public int Count => AllObjects.Count;

        public void Add(GameObject2D gameObject2D)
        {
            ArgumentNullException.ThrowIfNull(gameObject2D);
            var type = gameObject2D.GetType();
            mTypeDict.GetOrAdd(type, () => new()).Add(gameObject2D);
            AllObjects.Add(gameObject2D);
        }

        public void AddRange(IEnumerable<GameObject2D> gameObjects)
        {
            ArgumentNullException.ThrowIfNull(gameObjects);
            foreach (var gameObject in gameObjects) Add(gameObject);
        }

        public bool Remove(GameObject2D gameObject2D)
        {
            if (gameObject2D == null)
                return false;

            var type = gameObject2D.GetType();
            if (!(mTypeDict.TryGetValue(type, out var list) && list.Remove(gameObject2D))) return false;
            AllObjects.Remove(gameObject2D);
            return true;
        }

        public IEnumerable<T> OfType<T>() where T : GameObject2D
        {
            var type = typeof(T);
            ArgumentNullException.ThrowIfNull(type);
            if (type == typeof(GameObject2D))
                return AllObjects.OfType<T>();
            return mTypeDict.TryGetValue(type, out var list) ? list.OfType<T>() : new List<T>();
        }

        public void Sort(Comparison<GameObject2D> comparison)
        {
            foreach (var list in mTypeDict.Values)
                list.Sort(comparison);
        }

        public void Clear()
        {
            mTypeDict.Clear();
            AllObjects.Clear();
        }

        public void Initialize(SpatialHashing spatialHashing)
        {
            foreach (var obj in AllObjects)
                spatialHashing.InsertObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
        }

        public void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer)
        {
            var copyList = new List<GameObject2D>(AllObjects.ToList());
            foreach (var obj in copyList)
            {
                obj.Update(gameTime, inputState, gameLayer);

                if (!obj.IsDisposed) continue;
                if (!AllObjects.Remove(obj)) return;
                gameLayer.SpatialHashing.RemoveObject(obj, (int)obj.Position.X, (int)obj.Position.Y);
            }
        }

    }
}
