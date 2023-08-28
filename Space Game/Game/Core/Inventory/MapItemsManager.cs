using CelestialOdyssey.Game.Core.InputManagement;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.Inventory
{
    public enum ItemType
    {
        postyum,
        odyssyum
    }
    public class MapItemsManager
    {
        private List<Item> mItemsOnMap = new();

        public void Update(GameTime gameTime, InputState inputState)
        {

            for (int i = 0; i < mItemsOnMap.Count; i++)
            {
                var item = mItemsOnMap[i];
                if (item == null) continue;
                if (item.IsOnMap)
                {
                    item.Update(gameTime, inputState);
                    continue;
                }
                mItemsOnMap.Remove(item);
                item.RemoveFromSpatialHashing();
            }
        }

        public void SpawnItem(Vector2 position, ItemType type)
        {
            switch (type)
            {
                case ItemType.postyum:
                    mItemsOnMap.Add(new Postyum(position));
                    break;
                case ItemType.odyssyum:
                    mItemsOnMap.Add(new Odyssyum(position));
                    break;
            }
        }
    }
}
