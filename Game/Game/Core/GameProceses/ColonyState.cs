// ColonyState.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using System;

namespace StellarLiberation.Game.Core.GameProceses
{
    [Serializable]
    public class ColonyState
    {
        [JsonProperty] public readonly string Name;
        [JsonProperty] public int PopulationCount;
        // TODO
    }
}
