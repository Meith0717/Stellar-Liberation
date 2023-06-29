using System.Collections.Generic;

namespace Galaxy_Explovive.Core.PositionManagement
{
    public class SpatialHashing<T>
    {
        public int mCellSize;
        private Dictionary<int, List<T>> mPositionBuckets;

        public SpatialHashing(int cellSize)
        {
            mCellSize = cellSize;
            mPositionBuckets = new();
        }

        private int Hash(int xCoordinate, int yCoordinate)
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
