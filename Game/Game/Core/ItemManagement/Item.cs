// Item.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Layers;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace StellarLiberation.Game.Core.ItemManagement
{
    [Serializable]
    public abstract class Item : MovingObject
    {
        public readonly bool Collectable;

        protected Item(string textureId, float textureScale, bool collectable)
            : base(Vector2.Zero, textureId, textureScale, 30) { Collectable = collectable; }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            base.Update(gameTime, inputState, scene);
            Velocity = MovementController.GetVelocity(Velocity, 0, ExtendetRandom.Random.Next(5, 10) / 1000f);
        }

        public void Pull(Vector2 position)
        {
            var angleToPosition = Geometry.AngleBetweenVectors(Position, position);
            Direction = Geometry.CalculateDirectionVector(angleToPosition);
            Velocity = MovementController.GetVelocity(Velocity, float.PositiveInfinity, 0.015f);
        }

        public void Throw(Vector2 momentum, Vector2 position)
        {
            Position = position;
            ExtendetRandom.Random.NextUnitVector(out var direction);
            Direction = momentum + direction;
            Velocity = ExtendetRandom.Random.Next(10, 25) / 10;
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
