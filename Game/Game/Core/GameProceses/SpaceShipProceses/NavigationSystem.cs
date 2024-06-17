// NavigationSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.Visuals;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses
{
    [Serializable]
    public class NavigationSystem
    {
        [JsonProperty] public readonly List<WayPoint> WayPoints = new();
        [JsonProperty] private WayPoint? mActualWaypoint;

        public void AddWayPoint(Vector2 position, PlanetsystemState planetsystem)
            => WayPoints.Add(new(position, planetsystem));

        public void Update(Spacecraft spacecraft, ImpulseDrive impulseDrive, HyperDrive hyperDrive)
        {
            if (impulseDrive.IsMoving || hyperDrive.IsActive) 
                return;
            if (mActualWaypoint is not null)
                WayPoints.Remove((WayPoint)mActualWaypoint);
            if (WayPoints.Count == 0) return;
            mActualWaypoint = WayPoints.First();
            var actualWayPoint = (WayPoint)mActualWaypoint;
            if (actualWayPoint.TargetPlanetsystem.Contains(spacecraft))
            {
                impulseDrive.MoveToTarget(actualWayPoint.TargetPosition);
                return;
            }
            hyperDrive.SetTarget(actualWayPoint.TargetPlanetsystem);
        }

        public void DrawImpulseDriveWayPoints(Vector2 spacecraftPosition, PlanetsystemState currentPlanetsystem)
        {
            if (WayPoints.Count <= 0) return;
            var wayPoint = WayPoints.First();
            if (wayPoint.TargetPlanetsystem == currentPlanetsystem)
                ArrowPath.Draw(spacecraftPosition, wayPoint.TargetPosition, 20);
            for (int i = 1;  i < WayPoints.Count; i++)
            {
                wayPoint = WayPoints[i];
                var previousWayPont = WayPoints[i-1];
                if (wayPoint.TargetPlanetsystem == currentPlanetsystem)
                    ArrowPath.Draw(previousWayPont.TargetPosition, wayPoint.TargetPosition, 20);
            }
        }

        public void DrawHyperDriveWayPoints(PlanetsystemState currentPlanetsystem)
        {

        }

    }
}
