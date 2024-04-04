// MapState.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;

using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses
{
    [Serializable]
    public class MapState
    {
        [JsonIgnore] public readonly List<Planetsystem> Planetsystems = new();
        [JsonIgnore] public readonly SpatialHashing SpatialHasing = new(500);

        public void Initialize(List<PlanetsystemState> planetsystemStates)
        {
            foreach (var state in planetsystemStates)
            {
                var planetsystem = new Planetsystem(state);
                Planetsystems.Add(planetsystem);
                SpatialHasing.InsertObject(planetsystem, (int)planetsystem.Position.X, (int)planetsystem.Position.Y);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var planetSystem in Planetsystems)
                planetSystem.Update(gameTime, null, null);
        }
    }
}
