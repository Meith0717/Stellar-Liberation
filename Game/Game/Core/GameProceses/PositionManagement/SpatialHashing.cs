// SpatialHashing.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.PositionManagement
{
    /// <summary>
    /// Represents a spatial hashing data structure for efficient object retrieval based on their coordinates.
    /// </summary>
    /// <typeparam name="T">The type of objects stored in the spatial hashing structure.</typeparam>
    public class SpatialHashing
    {
        public int Count { get; private set; }
        public readonly int mCellSize;
        private readonly Dictionary<int, GameObject2DTypeList> mSpatialGrids = new();

        public SpatialHashing(int cellSize) => mCellSize = cellSize;

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

            return adjustedX / mCellSize * shiftingFactor + adjustedY / mCellSize;
        }

        public void InsertObject(GameObject2D obj, int x, int y)
        {
            var hash = Hash(x, y);
            if (!mSpatialGrids.TryGetValue(hash, out var objectBucket))
            {
                objectBucket = new();
                mSpatialGrids[hash] = objectBucket;
            }
            objectBucket.Add(obj);
            Count++;
        }

        public void RemoveObject(GameObject2D obj, int x, int y)
        {
            var hash = Hash(x, y);
            if (!mSpatialGrids.TryGetValue(hash, out var objectBucket)) return;
            if (!objectBucket.Remove(obj)) return;
            Count--;
            mSpatialGrids[hash] = objectBucket;
            if (objectBucket.Count == 0) mSpatialGrids.Remove(hash);
        }

        public void ClearBuckets() => mSpatialGrids.Clear();

        public bool TryGetObjectsInBucket(int x, int y, out GameObject2DTypeList object2Ds) => mSpatialGrids.TryGetValue(Hash(x, y), out object2Ds);

        public void GetObjectsInRadius<T>(Vector2 position, float radius, ref List<T> objectsInRadius, bool sortedByDistance = true) where T : GameObject2D
        {
            var startX = (int)Math.Floor((position.X - radius) / mCellSize);
            var endX = (int)Math.Ceiling((position.X + radius) / mCellSize);
            var startY = (int)Math.Floor((position.Y - radius) / mCellSize);
            var endY = (int)Math.Ceiling((position.Y + radius) / mCellSize);
            var xRange = Enumerable.Range(startX, endX - startX + 1);
            var yRange = Enumerable.Range(startY, endY - startY + 1);
            var lookUpCircle = new CircleF(position, radius);

            foreach (var x in xRange)
            {
                foreach (var y in yRange)
                {
                    if (!TryGetObjectsInBucket(x * mCellSize, y * mCellSize,out var objectsInBucket)) continue;
                    foreach (GameObject2D gameObject in objectsInBucket.OfType(typeof(T)))
                    {
                        if (!CircleF.Intersects(lookUpCircle, gameObject.BoundedBox)) continue;
                        objectsInRadius.Add((T)gameObject);
                    }
                }
            }

            if (!sortedByDistance) return;
            objectsInRadius.Sort((obj1, obj2) =>
            {
                var distance1 = Vector2.DistanceSquared(position, obj1.Position);
                var distance2 = Vector2.DistanceSquared(position, obj2.Position);
                return distance1.CompareTo(distance2);
            });
        }

        public List<T> GetObjectsInRadius<T>(Vector2 position, float radius, bool sortedByDistance = true) where T : GameObject2D
        {
            var objectsInRadius = new List<T>();
            GetObjectsInRadius<T>(position, radius, ref objectsInRadius, sortedByDistance);
            return objectsInRadius;
        }

        public void GetObjectsInRectangle<T>(RectangleF searchRectangle, ref List<T> objectsInRectangle) where T : GameObject2D
        {
            var startX = (int)Math.Floor(searchRectangle.Left / mCellSize);
            var endX = (int)Math.Ceiling(searchRectangle.Right / mCellSize);
            var startY = (int)Math.Floor(searchRectangle.Top / mCellSize);
            var endY = (int)Math.Ceiling(searchRectangle.Bottom / mCellSize);
            var xRange = Enumerable.Range(startX, endX - startX + 1);
            var yRange = Enumerable.Range(startY, endY - startY + 1);

            foreach (var x in xRange)
            {
                foreach (var y in yRange)
                {
                    if (!TryGetObjectsInBucket(x * mCellSize, y * mCellSize, out var objectsInBucket)) continue;
                    foreach (GameObject2D gameObject in objectsInBucket.OfType(typeof(T)))
                    {
                        if (!searchRectangle.Intersects(gameObject.BoundedBox))
                            continue;

                        objectsInRectangle.Add((T)gameObject);
                    }
                }
            }
        }

        public List<T> GetObjectsInRectangle<T>(RectangleF searchRectangle) where T : GameObject2D
        {
            var objectsInRadius = new List<T>();
            GetObjectsInRectangle<T>(searchRectangle, ref objectsInRadius);
            return objectsInRadius;
        }
    }
}
