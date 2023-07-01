using GalaxyExplovive.Core.GameEngine;
using GalaxyExplovive.Core.GameEngine.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalaxyExplovive.Game.GameLogik
{
    internal static class ObjectLocator
    {
        public static List<T> GetObjectsInRadius<T>(SpatialHashing<GameObject> sHashing, Vector2 positionVector2, int radius)
        {
            var objectsInRadius = new List<GameObject>();
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
            Comparison<GameObject> comparison = (a, b) => Vector2.Distance(a.Position, positionVector2).CompareTo(Vector2.Distance(b.Position, positionVector2));
            objectsInRadius.Sort(comparison);
            return objectsInRadius.OfType<T>().ToList();
        }
    }
}
