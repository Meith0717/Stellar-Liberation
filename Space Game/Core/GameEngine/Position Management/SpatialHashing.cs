/*
 *  SpatialHashing.cs
 *
 *  Copyright (c) 2023 Thierry Meiers
 *  All rights reserved.
 */

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyExplovive.Core.GameEngine.Position_Management
{
    /// <summary>
    /// Represents a spatial hashing data structure for efficient object retrieval based on their coordinates.
    /// </summary>
    /// <typeparam name="T">The type of objects stored in the spatial hashing structure.</typeparam>
    public class SpatialHashing<T>
    {
        /// <summary>
        /// The size of each cell in the spatial grid.
        /// </summary>
        public int mCellSize;

        /// <summary>
        /// The spatial grids that store the objects based on their hash values.
        /// </summary>
        private Dictionary<int, HashSet<T>> mSpatialGrids = new();

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
        private int Hash(int xCoordinate, int yCoordinate)
        {
            const int shiftingFactor = 10000;
            int shiftedX = xCoordinate + shiftingFactor / 2;
            int shiftedY = yCoordinate + shiftingFactor / 2;
            return shiftedX / mCellSize * shiftingFactor + shiftedY / mCellSize;
        }

        /// <summary>
        /// Inserts an object into the spatial hashing structure at the specified coordinates.
        /// </summary>
        /// <param name="myObject">The object to insert.</param>
        /// <param name="xCoordinate">The X coordinate.</param>
        /// <param name="yCoordinate">The Y coordinate.</param>
        public void InsertObject(T myObject, int x, int y)
        {
            var hash = Hash(x, y);
            if (!mSpatialGrids.ContainsKey(hash))
            {
                mSpatialGrids.Add(hash, new());
            }

            var objectBucket = mSpatialGrids[hash];
            if (objectBucket == null) return;

            objectBucket.Add(myObject);
            mSpatialGrids[hash] = objectBucket;
        }

        /// <summary>
        /// Removes an object from the spatial hashing structure at the specified coordinates.
        /// </summary>
        /// <param name="gameObject">The object to remove.</param>
        /// <param name="xCoordinate">The X coordinate.</param>
        /// <param name="yCoordinate">The Y coordinate.</param>
        public void RemoveObject(T gameObject, int x, int y)
        {
            var hash = Hash(x, y);
            if (!mSpatialGrids.ContainsKey(hash)) return;

            var objectBucket = mSpatialGrids[hash];
            if (objectBucket == null || objectBucket.Count == 0) return;

            objectBucket.Remove(gameObject);
            mSpatialGrids[hash] = objectBucket;
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

        /// <summary>
        /// Gets a list of objects in the specified space represented by a rectangle.
        /// </summary>
        /// <param name="space">The space represented by a rectangle.</param>
        /// <returns>A list of objects in the specified space.</returns>
        public List<T> GetObjectsInSpace(Rectangle space)
        {
            List<T> objects = new();
            for (int x = space.X; x <= space.X + space.Width + mCellSize; x += mCellSize)
            {
                for (int y = space.Y; y <= space.Y + space.Height + mCellSize; y += mCellSize)
                {
                    var objs = GetObjectsInBucket(x, y);
                    objects.AddRange(objs);
                }
            }
            return objects;
        }
    }
}
