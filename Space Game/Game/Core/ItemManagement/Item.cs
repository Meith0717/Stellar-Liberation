// Item.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.Utilitys;
using CelestialOdyssey.Game.Layers;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;

namespace CelestialOdyssey.Game.Core.ItemManagement
{
    [Serializable]
    public abstract class Item : MovingObject
    {
        public readonly bool Collectable;

        protected Item(string textureId, float textureScale, bool collectable)
            : base(Vector2.Zero, textureId, textureScale, 30) { Collectable = collectable; }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            base.Update(gameTime, inputState, gameLayer, scene);
            Velocity = MovementController.GetVelocity(Velocity, - ExtendetRandom.Random.Next(5, 10) / 1000f);
        }

        public void Pull(Vector2 position)
        {
            var angleToPosition = Geometry.AngleBetweenVectors(Position, position);
            Direction = Geometry.CalculateDirectionVector(angleToPosition);
            Velocity = MovementController.GetVelocity(Velocity, 0.015f);
        }

        public void Throw(Vector2 momentum, Vector2 position)
        {
            Position = position;
            ExtendetRandom.Random.NextUnitVector(out var direction);
            Direction = momentum + direction;
            Velocity = ExtendetRandom.Random.Next(5, 15) / 10f;
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
