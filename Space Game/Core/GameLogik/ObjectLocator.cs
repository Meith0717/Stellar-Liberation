using Galaxy_Explovive.Core.Map;
using Galaxy_Explovive.Core.PositionManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Explovive.Core.GameLogik
{
    internal class ObjectLocator
    {
        private static ObjectLocator mInstance = null;
        public static ObjectLocator Instance { get { return mInstance ??= new ObjectLocator(); } }
        
        public List<GameObject.GameObject> GetObjectsInRadius(Vector2 positionVector2, int radius)
        {
            var objectsInRadius = new List<GameObject.GameObject>();
            int CellSize = Globals.mGameLayer.mSpatialHashing.mCellSize;
            var maxRadius = radius + CellSize;
            for (var i = -radius; i <= maxRadius; i += CellSize)
            {
                for (var j = -radius; j <= maxRadius; j += CellSize)
                {
                    var objectsInBucket = Globals.mGameLayer.mSpatialHashing.GetObjectsInBucket((int)(positionVector2.X + i), (int)(positionVector2.Y + j));
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
            return objectsInRadius;
        }
    }
}
