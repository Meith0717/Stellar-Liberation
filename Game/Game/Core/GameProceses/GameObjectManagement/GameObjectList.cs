﻿// GameObjectList.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using StellarLiberation.Game.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.GameObjectManagement
{
    [Serializable]
    public class GameObjectList
    {
        [JsonProperty] private readonly Dictionary<Type, List<GameObject>> mTypeDict = new();
        [JsonProperty] private readonly List<GameObject> AllObjects = new();

        public int Count => AllObjects.Count;

        public List<GameObject> GameObject2Ds => AllObjects;

        public void Add(GameObject gameObject2D)
        {
            ArgumentNullException.ThrowIfNull(gameObject2D);
            var type = gameObject2D.GetType();
            mTypeDict.GetOrAdd(type, () => new()).Add(gameObject2D);
            AllObjects.Add(gameObject2D);
        }

        public void AddRange(IEnumerable<GameObject> gameObjects)
        {
            ArgumentNullException.ThrowIfNull(gameObjects);
            foreach (var gameObject in gameObjects) Add(gameObject);
        }

        public bool Remove(GameObject gameObject2D)
        {
            if (gameObject2D == null)
                return false;

            var type = gameObject2D.GetType();
            if (!(mTypeDict.TryGetValue(type, out var list) && list.Remove(gameObject2D))) return false;
            AllObjects.Remove(gameObject2D);
            return true;
        }

        public IEnumerable<T> OfType<T>() where T : GameObject
        {
            var type = typeof(T);
            ArgumentNullException.ThrowIfNull(type);
            if (type == typeof(GameObject))
                return AllObjects.OfType<T>();
            return mTypeDict.TryGetValue(type, out var list) ? list.OfType<T>() : new List<T>();
        }

        public void Sort(Comparison<GameObject> comparison)
        {
            foreach (var list in mTypeDict.Values)
                list.Sort(comparison);
        }

        public void Clear()
        {
            mTypeDict.Clear();
            AllObjects.Clear();
        }

        public bool Contains(GameObject gameObject2D) => AllObjects.Contains(gameObject2D);

        public List<GameObject> ListCopy() => new List<GameObject>(AllObjects);

    }
}
