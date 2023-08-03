using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.Inventory
{
    [Serializable]
    public class CargoHold
    {
        [JsonProperty] private readonly List<Item> mInventory = new();
        [JsonProperty] public int MaxMass { get; private set; }
        [JsonProperty] public int TotalMass { get; private set; }

        public CargoHold(int maxMass) 
        { 
            MaxMass = maxMass;
        }

        public bool RemoveItem(Item item) 
        {
            if (item == null) return false;
            if (!mInventory.Remove(item)) return false;
            TotalMass -= item.Mass;
            return true;
        }

        public bool AddItem(Item item)
        {
            if (item == null) return false;
            if (TotalMass + item.Mass > MaxMass) return false;
            TotalMass += item.Mass;
            mInventory.Add(item);
            return true;
        }

        public bool IsEmpty { get { return mInventory.Count == 0; } }
        public bool IsFull { get { return MaxMass == TotalMass;} }
    }
}
