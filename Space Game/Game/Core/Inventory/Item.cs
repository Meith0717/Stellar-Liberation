using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;

namespace CelestialOdyssey.Game.Core.Inventory
{
    [Serializable]
    public class Postyum : Item
    {
        public Postyum(Vector2 dropPosition) : base(dropPosition, ContentRegistry.postyum.Name, 1, 1) { }
    }

    [Serializable]
    public class Odyssyum : Item 
    {
        public Odyssyum(Vector2 dropPosition) : base(dropPosition, ContentRegistry.odyssyum.Name, 1, 1) { }
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

        public override void Update(GameTime gameTime, InputState inputState)
        {
            RemoveFromSpatialHashing();
            base.Update(gameTime, inputState);
            AddToSpatialHashing();
        }

        public override void Draw()
        {
            base.Draw();
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
