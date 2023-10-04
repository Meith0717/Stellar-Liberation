using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.LayerManagement;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.Core.ShipSystems
{
    public class SensorArray
    {

        public List<GameObject> SortedObjectsInRange = new();
        private int mScanRadius;

        private float mActualCounter;
        private float mMaxCounter = 1000;

        public SensorArray(int scanRadius, int scanCoolDown) 
        { 
            mScanRadius = scanRadius;
            mMaxCounter = scanCoolDown;
            mActualCounter = Utility.Utility.Random.Next(1000);
        }

        public void Update(GameTime gameTime, Vector2 Position, SceneLayer sceneLayer)
        {
            var planetSystem = spaceShip.ActualSystem;
            var position = spaceShip.Position;

            SortedObjectsInRange.Clear(); 
            if (planetSystem is null) return;
            SortedObjectsInRange.Add(planetSystem.Star);
            SortedObjectsInRange.AddRange(planetSystem.Planets);
            SortedObjectsInRange.AddRange(planetSystem.Pirates);
            Comparison<GameObject> comparison = (a, b) => Vector2.Distance(a.Position, position).CompareTo(Vector2.Distance(b.Position, position));
            SortedObjectsInRange.Remove(spaceShip);
            SortedObjectsInRange.Sort(comparison);
        }
    }
}
