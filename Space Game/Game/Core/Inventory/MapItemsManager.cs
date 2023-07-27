using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.Inventory
{
    public class MapItemsManager
    {
        private List<Item> mItemsOnMap = new();

        public void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            for (int i = 0; i < mItemsOnMap.Count; i++)
            {
                var item = mItemsOnMap[i];
                if (item == null) continue;
                if (item.LiveTime != 0)
                {
                    item.Update(gameTime, inputState, engine);
                    continue;
                }
                mItemsOnMap.Remove(item);
                item.RemoveFromSpatialHashing(engine);
            }
        }

        public void SpawnItem(Vector2 position)
        {
            mItemsOnMap.Add(new(position, "projectile", 0.5f, 0));
        }
    }
}
