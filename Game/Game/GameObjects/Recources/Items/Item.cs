// Item.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Components.PropulsionSystem;
using StellarLiberation.Game.Core.Utilitys;
using System;

namespace StellarLiberation.Game.GameObjects.Recources.Items
{
    [Serializable]
    public abstract class Item : GameObject2D
    {
        public readonly bool IsCollectable;
        public readonly bool IsStakable;

        public readonly ItemID ItemID;
        public int Amount;

        protected Item(ItemID itemID, string textureId, float textureScale, bool isCollectable = true, bool isStackable = true)
            : base(Vector2.Zero, textureId, textureScale, 30)
        {
            Amount = 1;
            IsCollectable = isCollectable;
            IsStakable = isStackable;
            ItemID = itemID;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            Velocity = MovementController.GetVelocity(Velocity, 0, ExtendetRandom.Random.Next(5, 10) / 1000f);
            GameObject2DMover.Move(gameTime, this, scene);
            base.Update(gameTime, inputState, scene);
        }

        public void Pull(Vector2 position)
        {
            var angleToPosition = Geometry.AngleBetweenVectors(Position, position);
            MovingDirection = Geometry.CalculateDirectionVector(angleToPosition);
            Velocity = MovementController.GetVelocity(Velocity, float.PositiveInfinity, 0.03f);
        }

        public void Throw(Vector2 momentum, Vector2 position)
        {
            Position = position;
            ExtendetRandom.Random.NextUnitVector(out var direction);
            MovingDirection = momentum + direction;
            Velocity = ExtendetRandom.Random.Next(10, 25) / 10;
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
