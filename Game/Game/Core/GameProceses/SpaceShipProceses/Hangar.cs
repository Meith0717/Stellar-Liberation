// Hangar.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using StellarLiberation.Game.GameObjects.SpaceCrafts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses
{
    [Serializable]
    public class Hangar
    {
        [JsonProperty] public int Capacity { get; private set; }
        [JsonProperty] public readonly List<Battleship> InSpaceShips = new();
        [JsonProperty] private readonly Dictionary<string, int> mOnBoardShips = new();
        [JsonProperty] private readonly Dictionary<string, int> mAssignedShips = new();

        [JsonIgnore] public int Count => mOnBoardShips.Sum((key) => key.Value);

        public Hangar(int capacity)
            => Capacity = capacity;

        public void Boost(float CapacityPerc)
            => Capacity = (int)(CapacityPerc * Capacity);

        public bool HasFreeSlot()
            => Count < Capacity;

        public int FreeSpace
            => Capacity - Count;

        public bool BoardAssignedShip(Battleship battleship)
        {
            if (!InSpaceShips.Contains(battleship))
                throw new Exception("Try to board a not assigned ship to the hangar");
            var shipID = battleship.ID;
            if (!mAssignedShips.TryGetValue(shipID, out var assignedAmount))
                throw new Exception("Try to board a not assigned ship to the hangar");
            if (!mOnBoardShips.TryGetValue(shipID, out var onBoardAmount)) return false;
            if (onBoardAmount == assignedAmount)
                throw new Exception($"All assigned ships of ID {shipID} on board!");
            battleship.IsDisposed = true;
            mOnBoardShips[shipID]++;
            InSpaceShips.Remove(battleship);
            return true;
        }

        public bool AssignNewShip(string battleshipID, int amount)
        {
            if (Count + amount > Capacity) return false;
            mAssignedShips.GetOrAdd(battleshipID, () => 0);
            mAssignedShips[battleshipID] += amount;
            mOnBoardShips.GetOrAdd(battleshipID, () => 0);
            mOnBoardShips[battleshipID] += amount;
            return true;
        }

        public int GetAssignedAmount(string battleshipId) => mAssignedShips.TryGetValue(battleshipId, out var amount) ? amount : 0;
        public int GetOnBoardAmount(string battleshipId) => mOnBoardShips.TryGetValue(battleshipId, out var amount) ? amount : 0;


        [JsonIgnore] private readonly List<string> mSpawnShips = new();
        public void Spawn(string battleshipID) => mSpawnShips.Add(battleshipID);


        [JsonIgnore] private List<Battleship> OrderBackShips = new();
        public void OrderBack(string battleshipID) => OrderBackShips.AddRange(InSpaceShips.Where(obj => obj.ID == battleshipID));
        public void OrderAllBack() => OrderBackShips.AddRange(InSpaceShips);


        private bool Get(Vector2 position, string battleshipID, out Battleship battleship)
        {
            battleship = null;
            if (Count <= 0) return false;
            if (!mOnBoardShips.TryGetValue(battleshipID, out var amount)) return false;
            if (amount > 0)
            {
                battleship = BattleshipFactory.Instance.GetBattleShip(battleshipID, position, Fraction.Allied);
                mOnBoardShips[battleshipID]--;
                InSpaceShips.Add(battleship);
                return true;
            }
            return false;
        }

        public void Update(Flagship spacecraft, PlanetsystemState planetsystemState)
        {
            var position = ExtendetRandom.NextVectorOnBorder(spacecraft.BoundedBox);
            foreach (var shipID in mSpawnShips)
            {
                if (!Get(position, shipID, out var ship)) continue;
                planetsystemState.AddGameObject(ship);
            }
            mSpawnShips.Clear();

            foreach (var ship in InSpaceShips.ToList())
            {
                if (!ship.IsDisposed) continue;
                InSpaceShips.Remove(ship);
                mAssignedShips[ship.ID]--;
            }

            foreach (var ship in OrderBackShips)
                ship.Flagship = spacecraft;
            OrderBackShips.Clear();
        }
    }
}
