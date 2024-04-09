// Hangar.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Battleship Spawn()
        {
            var shipId = mOnBoardShips.First().Key;
            if (mOnBoardShips[shipId] <= 0) mOnBoardShips.Remove(shipId);
            return SpacecraftFactory.GetBattleship(Vector2.Zero, shipId, Fractions.Neutral);
        }
    }
}
