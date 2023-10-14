// Item.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems.PropulsionSystem;
using CelestialOdyssey.Game.Core.Utility;
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

        public override void Update(GameTime gameTime, InputState inputState, SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Update(gameTime, inputState, sceneManagerLayer, scene);
            Velocity = MovementController.GetVelocity(Velocity, - Utility.Utility.Random.Next(5, 10) / 1000f);
        }

        public void Pull(Vector2 position)
        {
            var angleToPosition = Utility.Geometry.AngleBetweenVectors(Position, position);
            Direction = Utility.Geometry.CalculateDirectionVector(angleToPosition);
            Velocity = MovementController.GetVelocity(Velocity, 0.015f);
        }

        public void Throw(Vector2 momentum, Vector2 position)
        {
            Position = position;
            Utility.Utility.Random.NextUnitVector(out var direction);
            Direction = momentum + direction;
            Velocity = Utility.Utility.Random.Next(5, 15) / 10f;
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
