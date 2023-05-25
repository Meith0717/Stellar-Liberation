using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.GameLogik
{
    internal class ObjectLocator
    {
        private static ObjectLocator mInstance = null;
        public static ObjectLocator Instance { get { return mInstance ??= new ObjectLocator(); } }

        public List<GameObject.GameObject> GetObjectsInRadius(Vector2 positionVector2, int radius)
        {
            var objectsInRadius = new List<GameObject.GameObject>();
            int CellSize = Globals.GameLayer.mSpatialHashing.mCellSize;
            var maxRadius = radius + CellSize;
            for (var i = -radius; i <= maxRadius; i += CellSize)
            {
                for (var j = -radius; j <= maxRadius; j += CellSize)
                {
                    var objectsInBucket = Globals.GameLayer.mSpatialHashing.GetObjectsInBucket((int)(positionVector2.X + i), (int)(positionVector2.Y + j));
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
            return objectsInRadius;
        }
    }
}
