// Hangar.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Newtonsoft.Json;
using StellarLiberation.Game.GameObjects.SpaceCrafts;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipComponents
{
    [Serializable]
    public class Hangar
    {
        [JsonProperty] private readonly Dictionary<BattleshipID, int> mOnBoardShips = new();
        [JsonProperty] public int Capacity;

        public bool Add(BattleshipID shipID)
        {
            if (mOnBoardShips.Count >= Capacity) return false;
            mOnBoardShips[shipID]++;
            return true;
        }
    }
}
