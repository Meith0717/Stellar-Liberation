using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace rache_der_reti.Core.PositionManagement
{
    [Serializable]
    public class SpatialHashing<T>
    {
        [JsonProperty]
        private int mCellSize;

        [JsonProperty]
        private Dictionary<int, List<T>> mPositionBuckets;

        [JsonProperty]
        private int mCount;

        public SpatialHashing(int cellSize)
        {
            mCellSize = cellSize;
            mPositionBuckets = new Dictionary<int, List<T>>();
        }

        public int Hash(int xCoordinate, int yCoordinate)
        {
            const int shiftingFactor = 10000;
            return xCoordinate / mCellSize * shiftingFactor + yCoordinate / mCellSize;
        }

        public void InsertObject(T myObject, int xCoordinate, int yCoordinate)
        {
            var hash = Hash(xCoordinate, yCoordinate);
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
            mCount++;
            mPositionBuckets[hash] = objectBucket;
        }

        public void RemoveObject(T gameObject, int xCoordinate, int yCoordinate)
        {
            var hash = Hash(xCoordinate, yCoordinate);
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
            mPositionBuckets = mPositionBuckets = new Dictionary<int, List<T>>();
        }

        public List<T> GetObjectsInBucket(int xCoordinate, int yCoordinate)
        {
            var hash = Hash(xCoordinate, yCoordinate);
            return mPositionBuckets.TryGetValue(hash, out var objectsInBucket) ? objectsInBucket : new List<T>();
        }
    }
}
