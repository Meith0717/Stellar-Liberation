using System.Collections;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.Inventory
{
    public class Inventory
    {
        private List<Item> mInventory = new();
        private SortedSet<int> mFreeSlots = new();

        public Inventory(int size) 
        {
            for (int i = 0; i < size; i++)
            {
                mInventory.Add(null);
                mFreeSlots.Add(i);
            }
        }

        public bool RemoveItem(Item item) 
        {
            var slot = mInventory.IndexOf(item);
            mInventory[slot] = null;
            mFreeSlots.Add(slot);
            System.Diagnostics.Debug.WriteLine(ToString());
            return true;
        }

        public bool AddItem(Item item)
        {
            if (mFreeSlots.Count == 0) return false;
            var smalestFreeSlot = mFreeSlots.Min;
            mInventory[smalestFreeSlot] = item;
            mFreeSlots.Remove(smalestFreeSlot);
            System.Diagnostics.Debug.WriteLine(ToString());
            return true;
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < mInventory.Count; i++)
            {
                Item item = mInventory[i];
                string name = (item == null) ? "null" : item.GetType().Name;
                s += $"{i}: {name} | ";
            }
            return s;
        }
    }
}
