// MapConfig.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarLiberation.Game.Core.GameProceses.MapGeneration
{
    [Serializable]
    public class MapConfig
    {
        [JsonProperty] public readonly int SectorCountWidth;
        [JsonProperty] public readonly int SectorCountHeight;
        [JsonProperty] public readonly int Seed;

        public MapConfig(int sectorCountWidth, int sectorCountHeight, int seed)
        {
            SectorCountWidth = sectorCountWidth;
            SectorCountHeight = sectorCountHeight;
            Seed = seed;
        }
    }
}
