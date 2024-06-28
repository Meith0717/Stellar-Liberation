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
        [JsonIgnore] private PlanetsystemState mCurrentPlanetsystem;

        public void NewWayPoint(Vector2 currentPosition, Vector2 targetPosition, PlanetsystemState targetPlanetsystem)
        {

        }

        public void AddWayPoint(Vector2 currentPosition, Vector2 targetPosition, PlanetsystemState targetPlanetsystem)
        {
            var lastTargetPosition = currentPosition;
            if (WayPoints.Count > 0)
                lastTargetPosition = WayPoints.Last().TargetPosition;
            if (mCurrentPlanetsystem != targetPlanetsystem)
                WayPoints.Add(new(lastTargetPosition, targetPlanetsystem));
            WayPoints.Add(new(targetPosition, targetPlanetsystem));
        }

        public void Update(Spacecraft spacecraft, ImpulseDrive impulseDrive, HyperDrive hyperDrive, PlanetsystemState currentPlanetsystem)
        {
            mCurrentPlanetsystem = currentPlanetsystem;
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

        public void DrawImpulseDriveWayPoints(Spacecraft spacecraft, PlanetsystemState focusedPlanetsystem)
        {
            if (WayPoints.Count <= 0) return;
            var wayPoint = WayPoints.First();
            if (wayPoint.TargetPlanetsystem == focusedPlanetsystem)
                ArrowPath.Draw(spacecraft, wayPoint.TargetPosition, 10);
            for (int i = 1; i < WayPoints.Count; i++)
            {
                var previousWayPont = WayPoints[i - 1];
                wayPoint = WayPoints[i];
                if (wayPoint.TargetPlanetsystem != focusedPlanetsystem) continue;
                if (previousWayPont.TargetPlanetsystem == wayPoint.TargetPlanetsystem)
                    ArrowPath.Draw(previousWayPont.TargetPosition, wayPoint.TargetPosition, 10);
            }
        }

        public void DrawHyperDriveWayPoints()
        {
            if (WayPoints.Count <= 0) return;
            var wayPoint = WayPoints.First();
            if (wayPoint.TargetPlanetsystem != mCurrentPlanetsystem)
                ArrowPath.Draw(mCurrentPlanetsystem.MapPosition, wayPoint.TargetPlanetsystem.MapPosition, .2f);
            for (int i = 1; i < WayPoints.Count; i++)
            {
                wayPoint = WayPoints[i];
                var previousWayPont = WayPoints[i - 1];
                if (wayPoint.TargetPlanetsystem != previousWayPont.TargetPlanetsystem)
                    ArrowPath.Draw(previousWayPont.TargetPlanetsystem.MapPosition, wayPoint.TargetPlanetsystem.MapPosition, .2f);
            }
        }
    }
}
