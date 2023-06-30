using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Explovive.Core.SpatialHashing
{
    internal class SpatialGrid<T>
    {
        public Rectangle Grid { get; private set; }
        public HashSet<T> Objects = new();

        public SpatialGrid(int hash, int size) 
        {
            
        }

        private Vector2 GetGridCoordinates(int hash, int size)
        {
            Vector2 position = new();

            const int shiftingFactor = 10000;

            int shiftedHash = hash / shiftingFactor;
            int shiftedX = shiftedHash % size;
            int shiftedY = shiftedHash / size;

            position.X = shiftedX - (shiftingFactor / 2);
            position.Y = shiftedY - (shiftingFactor / 2);
            return position;
        }
    }
}
