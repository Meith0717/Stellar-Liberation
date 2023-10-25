// QuantumGate.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjectManagement;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Layers;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System.Linq;

namespace CelestialOdyssey.Game.GameObjects
{
    public class QuantumGate : GameObject
    {

        public QuantumGate(Vector2 position)
            : base(position, ContentRegistry.placeHolder, 10, 50) 
        { }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            RemoveFromSpatialHashing(scene);
            base.Update(gameTime, inputState, gameLayer, scene);
            AddToSpatialHashing(scene);

            var players = scene.GetObjectsInRadius<Player>(Position, (int)BoundedBox.Diameter);
            if (!players.Any()) return;
            if (players.First().HyperDrive.TargetPlanetSystem is null) return;
            gameLayer.PopScene();
            gameLayer.LoadPlanetSystem(players.First().HyperDrive.TargetPlanetSystem);
            players.First().HyperDrive.TargetPlanetSystem = null;
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
