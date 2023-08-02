using System.Collections;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.Inventory
{
    public class CargoHold
    {
        private List<Item> mInventory = new();
        public int MaxMass { get; private set; }
        public int TotalMass { get; private set; }

        public CargoHold(int maxMass) { MaxMass = maxMass; }

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

        public override string ToString()
        {
            string s = "";
            foreach (var item in mInventory) { s += $"{item.GetType().Name}, "; }
            return s;
        }
    }
}
