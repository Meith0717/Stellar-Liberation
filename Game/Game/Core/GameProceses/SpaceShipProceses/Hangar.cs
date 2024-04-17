// Hangar.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.Utilitys;
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
        [JsonProperty] private readonly List<BattleshipID> mOrderOutShips = new();

        [JsonIgnore] public int Count => mOnBoardShips.Sum((key) => key.Value);

        public Hangar(int capacity) => Capacity = capacity;

        public void Boost(float CapacityPerc) => Capacity = (int)(CapacityPerc * Capacity);

        public bool HasFreeSlot() => Count < Capacity;

        public int FreeSpace => Capacity - Count;

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

        public void Spawn(BattleshipID battleshipID) => mOrderOutShips.Add(battleshipID);

        private bool Get(Vector2 position, BattleshipID battleshipID, out Battleship battleship)
        {
            battleship = null;
            if (Count <= 0) return false;
            if (!mOnBoardShips.TryGetValue(battleshipID, out var amount)) return false;
            if (amount > 0)
            {
                battleship = SpacecraftFactory.GetBattleship(position, battleshipID, Fractions.Allied);                
                mOnBoardShips[battleshipID]--;
                return true;
            } 
            mOnBoardShips.Remove(battleshipID);
            return false;
        }


        public void Update(Spacecraft spacecraft, PlanetsystemState planetsystemState)
        {
            var position = ExtendetRandom.NextVectorOnBorder(spacecraft.BoundedBox);
            foreach (var shipID in mOrderOutShips)
            {
                if (!Get(position, shipID, out var ship)) continue;
                planetsystemState.AddGameObject(ship);
            }
            mOrderOutShips.Clear();
        }
    }
}
