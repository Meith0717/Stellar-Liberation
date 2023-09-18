using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.ShipSystems
{
    public class SensorArray
    {

        public List<GameObject> ObjectsInRange = new();
        private int mScanRadius;

        public SensorArray(int scanRadius) { mScanRadius = scanRadius; }

        public void Update(Vector2 Position, SceneLayer sceneLayer)
        {
            ObjectsInRange = sceneLayer.GetSortedObjectsInRadius<GameObject>(Position, mScanRadius);
        }

    }
}
