// SpatialHashing.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

/*
 *  SpatialHashing.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.GameObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Core.GameEngine.Position_Management
{
    /// <summary>
    /// Represents a spatial hashing data structure for efficient object retrieval based on their coordinates.
    /// </summary>
    /// <typeparam name="T">The type of objects stored in the spatial hashing structure.</typeparam>
    [Serializable]
    public class SpatialHashing<T> where T : GameObject2D
    {
        [JsonProperty] public int CellSize;
        [JsonProperty] private Dictionary<int, HashSet<T>> mSpatialGrids = new();

        public SpatialHashing(int cellSize) => CellSize = cellSize;

        public int Hash(int xCoordinate, int yCoordinate)
        {
            const int shiftingFactor = 10000;

            var WorldWidth = 100000000;
            var WorldHeight = 100000000;

            // Adjust coordinates relative to the center of the world
            int adjustedX = xCoordinate + WorldWidth / 2;
            int adjustedY = yCoordinate + WorldHeight / 2;

            // Ensure that adjustedX and adjustedY are non-negative
            if (adjustedX < 0) adjustedX += WorldWidth;
            if (adjustedY < 0) adjustedY += WorldHeight;

            return (adjustedX / CellSize) * shiftingFactor + (adjustedY / CellSize);
        }


        public void InsertObject(T obj, int x, int y)
        {
            var hash = Hash(x, y);
            if (!mSpatialGrids.TryGetValue(hash, out var objectBucket))
            {
                objectBucket = new HashSet<T>();
                mSpatialGrids[hash] = objectBucket;
            }
            objectBucket.Add(obj);
        }

        public void RemoveObject(T obj, int x, int y)
        {
            var hash = Hash(x, y);
            if (!mSpatialGrids.TryGetValue(hash, out var objectBucket)) return;
            objectBucket.Remove(obj);
            mSpatialGrids[hash] = objectBucket;
            if (objectBucket.Count == 0) mSpatialGrids.Remove(hash);
        }

        public void ClearBuckets() => mSpatialGrids.Clear();

        public List<T> GetObjectsInBucket(int x, int y)
        {
            var hash = Hash(x, y);
            return mSpatialGrids.TryGetValue(hash, out var objectsInBucket) ? objectsInBucket.ToList() : new List<T>();
        }

        public List<T> GetObjectsInRadius<T>(Vector2 position, float radius, bool sortedByDistance = true) where T : GameObject2D
        {
            // Determine the range of bucket indices that fall within the radius.
            var startX = (int)Math.Floor((position.X - radius) / CellSize);
            var endX = (int)Math.Ceiling((position.X + radius) / CellSize);
            var startY = (int)Math.Floor((position.Y - radius) / CellSize);
            var endY = (int)Math.Ceiling((position.Y + radius) / CellSize);

            List<T> objectsInRadius = new List<T>();

            foreach (var x in Enumerable.Range(startX, endX - startX + 1))
            {
                foreach (var y in Enumerable.Range(startY, endY - startY + 1))
                {
                    var objectsInBucket = GetObjectsInBucket(x * CellSize, y * CellSize);
                    foreach (var gameObject in objectsInBucket.OfType<T>())
                    {
                        var circle = new CircleF(position, radius);
                        if (CircleF.Intersects(circle, gameObject.BoundedBox)) objectsInRadius.Add(gameObject);
                    }
                }
            }

            if (!sortedByDistance) return objectsInRadius;

            // Sort the objects by distance to the specified position
            objectsInRadius.Sort((obj1, obj2) =>
            {
                var distance1 = Vector2.DistanceSquared(position, obj1.Position);
                var distance2 = Vector2.DistanceSquared(position, obj2.Position);
                return distance1.CompareTo(distance2);
            });

            return objectsInRadius;
        }
    }
}
