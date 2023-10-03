/*
 *  SpatialHashing.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Core.GameEngine.Position_Management
{
    /// <summary>
    /// Represents a spatial hashing data structure for efficient object retrieval based on their coordinates.
    /// </summary>
    /// <typeparam name="T">The type of objects stored in the spatial hashing structure.</typeparam>
    [Serializable]
    public class SpatialHashing<T>
    {

        /// <summary>
        /// The size of each cell in the spatial grid.
        /// </summary>
        [JsonProperty] public int mCellSize;

        /// <summary>
        /// The spatial grids that store the objects based on their hash values.
        /// </summary>
        [JsonProperty] private Dictionary<int, HashSet<T>> mSpatialGrids = new();

        /// <summary>
        /// Initializes a new instance of the SpatialHashing class with the specified cell size.
        /// </summary>
        /// <param name="cellSize">The size of each cell in the spatial grid.</param>
        public SpatialHashing(int cellSize)
        {
            mCellSize = cellSize;
        }

        /// <summary>
        /// Computes the hash value for the given coordinates.
        /// </summary>
        /// <param name="xCoordinate">The X coordinate.</param>
        /// <param name="yCoordinate">The Y coordinate.</param>
        /// <returns>The hash value for the given coordinates.</returns>
        public int Hash(int xCoordinate, int yCoordinate)
        {
            const int shiftingFactor = 10000;
            return xCoordinate / mCellSize * shiftingFactor + yCoordinate / mCellSize;
        }

        /// <summary>
        /// Inserts an object into the spatial hashing structure at the specified coordinates.
        /// </summary>
        /// <param name="obj">The object to insert.</param>
        /// <param name="xCoordinate">The X coordinate.</param>
        /// <param name="yCoordinate">The Y coordinate.</param>
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

        /// <summary>
        /// Removes an object from the spatial hashing structure at the specified coordinates.
        /// </summary>
        /// <param name="obj">The object to remove.</param>
        /// <param name="xCoordinate">The X coordinate.</param>
        /// <param name="yCoordinate">The Y coordinate.</param>
        public void RemoveObject(T obj, int x, int y)
        {
            var hash = Hash(x, y);
            if (!mSpatialGrids.TryGetValue(hash, out var objectBucket)) return;
            objectBucket.Remove(obj);
            mSpatialGrids[hash] = objectBucket;
            if (objectBucket.Count == 0) mSpatialGrids.Remove(hash);
        }

        /// <summary>
        /// Clears all the buckets in the spatial hashing structure.
        /// </summary>
        public void ClearBuckets()
        {
            mSpatialGrids = new();
        }

        /// <summary>
        /// Gets a list of objects in the specified bucket coordinates.
        /// </summary>
        /// <param name="xCoordinate">The X coordinate of the bucket.</param>
        /// <param name="yCoordinate">The Y coordinate of the bucket.</param>
        /// <returns>A list of objects in the specified bucket coordinates.</returns>
        public List<T> GetObjectsInBucket(int x, int y)
        {
            var hash = Hash(x, y);
            return mSpatialGrids.TryGetValue(hash, out var objectsInBucket) ? objectsInBucket.ToList() : new List<T>();
        }        

        public override string ToString()
        {
            string s = "";
            foreach (var grid in mSpatialGrids)
            {
                s += $"{grid.Key}[";
                foreach (var obj in grid.Value)
                {
                    s += $"{obj.GetType().Name},";
                }
                s += $"]\n";
            }
            return s;
        }
    }
}
