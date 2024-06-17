// WayPoint.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;


namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses
{
    public readonly struct WayPoint(Vector2 position, PlanetsystemState planetsystem)
    {
        public readonly Vector2 TargetPosition = position;
        public readonly PlanetsystemState TargetPlanetsystem = planetsystem;
    }
}
