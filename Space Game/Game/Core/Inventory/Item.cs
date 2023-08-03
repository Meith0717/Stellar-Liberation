using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.GameObjects;
using CelestialOdyssey.GameEngine.InputManagement;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.Inventory
{
    [Serializable]
    public class Postyum : Item
    {
        public Postyum(Vector2 dropPosition) : base(dropPosition, "postyum", 1, 1) { }
    }

    [Serializable]
    public class Odyssyum : Item 
    {
        public Odyssyum(Vector2 dropPosition) : base(dropPosition, "odyssyum", 1, 1) { }
    }

    public abstract class Item : GameObject
    {
        public int Mass { get; private set; }
        public int Price { get; private set; }

        public Item(Vector2 position, string textureId, int mass, int price) 
            : base(Vector2.Zero, textureId, 0.025f, 0) 
        {
            Drop(position);
            Price = price;
            Mass = mass;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameEngine.GameEngine engine)
        {
            RemoveFromSpatialHashing(engine);
            base.Update(gameTime, inputState, engine);
            AddToSpatialHashing(engine);
        }

        public override void Draw(GameEngine.GameEngine engine)
        {
            base.Draw(engine);
            TextureManager.Instance.DrawGameObject(this);
        }

        public bool IsOnMap { get; private set; }
        public void Collect()
        {
            IsOnMap = false;
            SoundManager.Instance.PlaySound("collect", 1);
        }
        public void Drop(Vector2 position) 
        { 
            Position = position;
            IsOnMap = true;
        }
        public float Sell()
        {
            IsOnMap = false;
            return Price;
        }
    }
}
