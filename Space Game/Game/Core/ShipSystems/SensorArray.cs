using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using System;
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

        public void Update(GameTime gameTime, Vector2 Position, Scene scene)
        {
            mActualCounter += gameTime.TotalGameTime.Milliseconds;
            if (mActualCounter < mMaxCounter) return;
            mActualCounter = 0;
            SortedObjectsInRange = scene.GetSortedObjectsInRadius<GameObject>(Position, mScanRadius);
            SortedObjectsInRange.Remove(SortedObjectsInRange.First());
        }
    }
}
