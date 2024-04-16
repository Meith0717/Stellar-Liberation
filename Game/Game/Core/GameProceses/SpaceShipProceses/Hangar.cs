// Hangar.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses
{
    [Serializable]
    public class Hangar
    {
        [JsonProperty] public int Capacity { get; private set; }
        [JsonProperty] private readonly Dictionary<BattleshipID, int> mOnBoardShips = new();

        [JsonIgnore] public int Count => mOnBoardShips.Sum((key) => key.Value);

        public Hangar(int capacity) => Capacity = capacity;

        public void Boost(float CapacityPerc) => Capacity = (int)(CapacityPerc * Capacity);

        public bool HasFreeSlot() => Count < Capacity;

        public bool Add(Battleship battleship)
        {
            var shipID = battleship.BattleshipID;
            battleship.IsDisposed = true;
            return Add(shipID, 1);
        }

        public bool Add(BattleshipID battleshipID, int amount)
        {
            if (Count + amount > Capacity) return false;
            mOnBoardShips.GetOrAdd(battleshipID, () => new());
            mOnBoardShips[battleshipID] += amount;
            return true;
        }

        public int GetAmount(BattleshipID battleshipId) 
        {
            if (mOnBoardShips.TryGetValue(battleshipId, out var amount))
                return amount;
            return 0;
        }

        public bool Get(Vector2 position, out Battleship battleship)
        {
            battleship = null;
            if (Count <= 0) return false;
            var shipId = mOnBoardShips.First().Key;
            mOnBoardShips[shipId]--;
            if (mOnBoardShips[shipId] <= 0) mOnBoardShips.Remove(shipId);
            battleship = SpacecraftFactory.GetBattleship(position, shipId, Fractions.Allied);
            return true;
        }

        public void Spawn(Vector2 position, PlanetsystemState planetsystemState)
        {
            Get(position, out var battleship);
            if (battleship is null) return;
            planetsystemState.AddGameObject(battleship);
        }
    }
}
