// SensorSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems
{
    public class SensorSystem
    {
        public int ShortRangeScanDistance { get; private set; }

        // Scan result
        public readonly List<GameObject2D> LongRangeScan = new();
        public readonly List<GameObject2D> ShortRangeScan = new();
        public readonly List<SpaceShip> OpponentsInRannge = new();
        public readonly List<SpaceShip> AlliesInRannge = new();

        public SensorSystem(int shortRangeScanDistance) => ShortRangeScanDistance = shortRangeScanDistance;

        public void Scan(Vector2 spaceShipPosition, Fractions fraction, Scene scene)
        {
            var planetSystem = scene.GameLayer.CurrentSystem;
            LongRangeScan.Clear();
            LongRangeScan.AddRange(planetSystem.GameObjects.Objects);

            ShortRangeScan.Clear();
            ShortRangeScan.AddRange(scene.SpatialHashing.GetObjectsInRadius<GameObject2D>(spaceShipPosition, ShortRangeScanDistance));

            OpponentsInRannge.Clear();
            OpponentsInRannge.AddRange(ShortRangeScan.OfType<SpaceShip>().ToList().Where((spaceShip) => spaceShip.Fraction != fraction));

            AlliesInRannge.Clear();
            AlliesInRannge.AddRange(ShortRangeScan.OfType<SpaceShip>().ToList().Where((spaceShip) => spaceShip.Fraction == fraction));
        }

        public void Draw(SpaceShip spaceShip, Scene scene) => TextureManager.Instance.DrawAdaptiveCircle(spaceShip.Position, ShortRangeScanDistance, new(50, 50, 50, 50), 2.5f, spaceShip.TextureDepth, scene.Camera2D.Zoom);

        public SpaceShip GetAimingShip(Vector2 spaceShipPosition, Fractions fraction)
        {
            SpaceShip spaceShip = null;
            PriorityQueue<SpaceShip, double> q = new();
            foreach (var spaceShip1 in OpponentsInRannge) q.Enqueue(spaceShip1, -GetAimingScore(spaceShipPosition, spaceShip1));
            q.TryDequeue(out spaceShip, out var _);
            return spaceShip;
        }

        private double GetAimingScore(Vector2 position, SpaceShip spaceShip)
        {
            var spaceShipShielHhullScore = spaceShip.DefenseSystem.ShieldPercentage * 0.5 + spaceShip.DefenseSystem.HullPercentage * 0.5;
            var distanceScore = Vector2.Distance(position, spaceShip.Position) / ShortRangeScanDistance;
            return (1 - spaceShipShielHhullScore) * 0.25 + (1 - distanceScore) * 0.75;
        }
    }
}
