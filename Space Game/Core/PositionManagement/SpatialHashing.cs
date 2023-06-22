using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxy_Explovive.Core.PositionManagement
{
    public class SpatialHashing<T>
    {
        public int mCellSize;
        private Dictionary<int, List<T>> mPositionBuckets;

        public SpatialHashing(int cellSize)
        {
            mCellSize = cellSize;
            mPositionBuckets = new Dictionary<int, List<T>>();
        }

        public void InsertObject(T myObject, Vector2 position)
        {
            var hash = Hash(position.X, position.Y);
            if (!mPositionBuckets.ContainsKey(hash))
            {
                mPositionBuckets.Add(hash, new List<T>());
            }

            var objectBucket = mPositionBuckets[hash];
            if (objectBucket == null)
            {
                return;
            }

            objectBucket.Add(myObject);
            mPositionBuckets[hash] = objectBucket;
        }

        public void RemoveObject(T gameObject, Vector2 position)
        {
            var hash = Hash(position.X, position.Y);
            if (!mPositionBuckets.ContainsKey(hash))
            {
                return;
            }

            var objectBucket = mPositionBuckets[hash];
            if (objectBucket == null || objectBucket.Count == 0)
            {
                return;
            }

            objectBucket.Remove(gameObject);


            mPositionBuckets[hash] = objectBucket;
        }

        public void ClearBuckets()
        {
            mPositionBuckets = new Dictionary<int, List<T>>();
        }

        public List<T> GetObjectsInBucket(int xCoordinate, int yCoordinate)
        {
            var hash = Hash(xCoordinate, yCoordinate);
            return mPositionBuckets.TryGetValue(hash, out var objectsInBucket) ? objectsInBucket : new List<T>();
        }

        private int Hash(float xCoordinate, float yCoordinate)
        {
            const int shiftingFactor = 10000;
            return (int)xCoordinate / mCellSize * shiftingFactor + (int)yCoordinate / mCellSize;
        }

        public static List<T> GetObjectsInRadius<T>(SpatialHashing<GameObject.GameObject> sHashing, Vector2 positionVector2, int radius)
        {
            var objectsInRadius = new List<GameObject.GameObject>();
            int CellSize = sHashing.mCellSize;
            var maxRadius = radius + CellSize;
            for (var i = -radius; i <= maxRadius; i += CellSize)
            {
                for (var j = -radius; j <= maxRadius; j += CellSize)
                {
                    var objectsInBucket = sHashing.GetObjectsInBucket((int)(positionVector2.X + i), (int)(positionVector2.Y + j));
                    foreach (var gameObject in objectsInBucket)
                    {
                        var position = gameObject.Position;
                        var distance = Vector2.Distance(positionVector2, position);
                        if (distance <= radius)
                        {
                            objectsInRadius.Add(gameObject);
                        }
                    }
                }

            }
            Comparison<GameObject.GameObject> comparison = (a, b) => Vector2.Distance(a.Position, positionVector2).CompareTo(Vector2.Distance(b.Position, positionVector2));
            objectsInRadius.Sort(comparison);
            return objectsInRadius.OfType<T>().ToList();
        }
    }
}
