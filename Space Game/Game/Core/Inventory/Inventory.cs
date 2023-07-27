using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CelestialOdyssey.Game.Core.Inventory
{
    public class Inventory
    {
        private List<Item> mInventory = new();
        private SortedSet<int> freeSlots = new();

        public Inventory(int size) 
        {
            for (int i = 0; i < 5 * 5; i++) 
            {
                mInventory[i] = null;
                freeSlots.Add(i);
            }
        }

        public void Update()
        {

        }
    }
}
