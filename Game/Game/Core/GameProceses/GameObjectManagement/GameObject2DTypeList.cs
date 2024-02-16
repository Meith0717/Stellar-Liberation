// GameObject2DTypeList.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    [Serializable]
    public class GameObject2DTypeList : IEnumerable<GameObject2D>
    {
        [JsonProperty] public int Count => mValues.Count;
        [JsonProperty] private readonly Dictionary<Type, HashSet<GameObject2D>> mKeyValuePairs;
        [JsonProperty] private readonly List<GameObject2D> mCollidables;
        [JsonProperty] private readonly List<GameObject2D> mValues;

        public GameObject2DTypeList()
        {
            mKeyValuePairs = new();
            mValues = new();
            mCollidables = new();
        }

        public void Add(GameObject2D gameObject2D)
        {
            var type = gameObject2D.GetType();
            mKeyValuePairs.GetOrAdd(type, () => new()).Add(gameObject2D);
            if (gameObject2D is ICollidable)
                mCollidables.Add(gameObject2D);
            mValues.Add(gameObject2D);
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
            if (!(mKeyValuePairs.TryGetValue(type, out var list) && list.Remove(gameObject2D))) return false;
            mCollidables.Remove(gameObject2D);
            mValues.Remove(gameObject2D);
            return true;
        }

        public IEnumerable<GameObject2D> OfType(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);
            if (type == typeof(GameObject2D))
                return mValues;
            if (type == typeof(ICollidable))
                return mCollidables;

            return mKeyValuePairs.TryGetValue(type, out var list) ? list : new();
        }

        public List<GameObject2D> ToList() => mValues;

        public IEnumerator<GameObject2D> GetEnumerator() => mValues.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
