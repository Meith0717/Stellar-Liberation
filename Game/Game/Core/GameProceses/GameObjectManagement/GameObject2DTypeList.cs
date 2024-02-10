// GameObject2DTypeList.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
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
        [JsonProperty] private readonly HashSet<GameObject2D> mValues;

        public GameObject2DTypeList()
        {
            mKeyValuePairs = new();
            mValues = new();
        }

        public void Add(GameObject2D gameObject2D)
        {
            var type = gameObject2D.GetType();
            mKeyValuePairs.GetOrAdd(type, () => new()).Add(gameObject2D);
            mValues.Add(gameObject2D);
        }

        public void AddRange(IEnumerable<GameObject2D> gameObjects)
        {
            if (gameObjects == null)
                throw new ArgumentNullException(nameof(gameObjects));

            foreach (var gameObject in gameObjects) Add(gameObject);
        }

        public bool Remove(GameObject2D gameObject2D)
        {
            if (gameObject2D == null)
                return false;

            var type = gameObject2D.GetType();
            if (!(mKeyValuePairs.TryGetValue(type, out var list) && list.Remove(gameObject2D))) return false;
            mValues.Remove(gameObject2D);
            return true;
        }

        public HashSet<GameObject2D> OfType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type == typeof(GameObject2D))
                return mValues;

            return mKeyValuePairs.TryGetValue(type, out var list) ? list : new();
        }

        public List<GameObject2D> ToList() => mValues.ToList();

        public IEnumerator<GameObject2D> GetEnumerator() => mValues.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;

            value = valueFactory();
            dictionary[key] = value;
            return value;
        }
    }
}
