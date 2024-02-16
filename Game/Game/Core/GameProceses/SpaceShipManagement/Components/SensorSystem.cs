// SensorSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components
{
    public class SensorSystem
    {
        private const int MaxCoolDown = 500;

        public int ShortRangeScanDistance { get; private set; }
        public GameObject2D FocusObject { get; private set; }
        public List<GameObject2D> LongRangeScan => mLongRangeScan;
        public List<SpaceShip> OpponentsInRannge => mOpponentsInRannge;
        public List<SpaceShip> AlliesInRannge => mAlliesInRannge;

        private List<GameObject2D> mLongRangeScan = new();
        private List<SpaceShip> mOpponentsInRannge = new();
        private List<SpaceShip> mAlliesInRannge = new();

        private int mCoolDown;

        public SensorSystem(int shortRangeScanDistance)
        {
            mCoolDown = ExtendetRandom.Random.Next(MaxCoolDown);
            ShortRangeScanDistance = shortRangeScanDistance;
        }

        // OPTIMIZE
        public void Scan(GameTime gameTime, PlanetSystem planetSystem, Vector2 spaceShipPosition, Fractions fraction, GameLayer scene)
        {
            mCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            if (mCoolDown > 0) return;
            mCoolDown = MaxCoolDown;

            mLongRangeScan = planetSystem.AstronomicalObjs.ToList();

            List<SpaceShip> shortRangeScan = new();
            scene.SpatialHashing.GetObjectsInRadius(spaceShipPosition, ShortRangeScanDistance, ref shortRangeScan);

            mOpponentsInRannge = shortRangeScan.Where((spaceShip) => spaceShip.Fraction != fraction).ToList();
            mAlliesInRannge = shortRangeScan.Where((spaceShip) => spaceShip.Fraction == fraction).ToList();
        }

        public void Draw(SpaceShip spaceShip, GameLayer scene) => TextureManager.Instance.DrawAdaptiveCircle(spaceShip.Position, ShortRangeScanDistance, new(50, 50, 50, 50), 2.5f, spaceShip.TextureDepth, scene.Camera2D.Zoom);

        public SpaceShip GetAimingShip(Vector2 spaceShipPosition)
        {
            PriorityQueue<SpaceShip, double> q = new();
            foreach (var spaceShip1 in mOpponentsInRannge)
                q.Enqueue(spaceShip1, -GetAimingScore(spaceShipPosition, spaceShip1));
            q.TryDequeue(out var spaceShip, out var _);
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
