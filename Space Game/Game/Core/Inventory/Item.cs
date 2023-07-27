using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;

namespace CelestialOdyssey.Game.Core.Inventory
{
    internal class Item : GameObject
    {
        public float LiveTime = 120;

        public Item(Vector2 position, string textureId, float textureScale, int textureDepth) 
            : base(position, textureId, textureScale, textureDepth) { }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            RemoveFromSpatialHashing(engine);
            LiveTime -= gameTime.ElapsedGameTime.Milliseconds / 1000;
            var objects = engine.GetObjectsInRadius<Player>(Position, 500);
            if (objects.Count != 0 && objects[0].BoundedBox.Contains(Position))
            {
                if (objects[0].BoundedBox.Contains(Position))
                {
                    LiveTime = 0;
                    SoundManager.Instance.PlaySound("collect", 1);
                }  
            }
            base.Update(gameTime, inputState, engine);
            AddToSpatialHashing(engine);
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
