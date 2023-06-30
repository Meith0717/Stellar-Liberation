using Galaxy_Explovive.Core.Rendering;
using Galaxy_Explovive.Core.TextureManagement;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Galaxy_Explovive.Core.PositionManagement
{
    public class SpatialHashing<T>
    {
        public int mCellSize;
        private Dictionary<int, HashSet<T>> mSpatialGrids;

        public SpatialHashing(int cellSize)
        {
            mCellSize = cellSize;
            mSpatialGrids = new();
        }

        private int Hash(int xCoordinate, int yCoordinate)
        {
            const int shiftingFactor = 10000;
            int shiftedX = xCoordinate + shiftingFactor / 2;
            int shiftedY = yCoordinate + shiftingFactor / 2;
            return shiftedX / mCellSize * shiftingFactor + shiftedY / mCellSize;
        }

        private Vector2 GetGridCoordinates(int hash, int size)
        {
            Vector2 position = new();

            const int shiftingFactor = 10000;

            int shiftedHash = hash / shiftingFactor;
            int shiftedX = shiftedHash % size;
            int shiftedY = shiftedHash / size;

            position.X = shiftedX - (shiftingFactor / 2);
            position.Y = shiftedY - (shiftingFactor / 2);
            return position;
        }

        public void InsertObject(T myObject, int xCoordinate, int yCoordinate)
        {
            var hash = Hash(xCoordinate, yCoordinate);
            if (!mSpatialGrids.ContainsKey(hash))
            {
                mSpatialGrids.Add(hash, new());
            }

            var objectBucket = mSpatialGrids[hash];
            if (objectBucket == null)
            {
                return;
            }

            objectBucket.Add(myObject);
            mSpatialGrids[hash] = objectBucket;
        }

        public void RemoveObject(T gameObject, int xCoordinate, int yCoordinate)
        {
            var hash = Hash(xCoordinate, yCoordinate);
            if (!mSpatialGrids.ContainsKey(hash))
            {
                return;
            }

            var objectBucket = mSpatialGrids[hash];
            if (objectBucket == null || objectBucket.Count == 0)
            {
                return;
            }

            objectBucket.Remove(gameObject);

            mSpatialGrids[hash] = objectBucket;
        }

        public void ClearBuckets()
        {
            mSpatialGrids = mSpatialGrids = new();
        }

        public List<T> GetObjectsInBucket(int xCoordinate, int yCoordinate)
        {
            var hash = Hash(xCoordinate, yCoordinate);
            return mSpatialGrids.TryGetValue(hash, out var objectsInBucket) ? objectsInBucket.ToList<T>() : new List<T>();
        }

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

        public override string ToString()
        {
            string s = "";
            foreach (var bucked in mSpatialGrids)
            {
                s += $"{bucked}, ";
            }
            return s;
        }
    }
}
