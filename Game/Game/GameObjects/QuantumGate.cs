// QuantumGate.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Layers;
using System.Linq;

namespace StellarLiberation.Game.GameObjects
{
    public class QuantumGate : GameObject
    {

        public QuantumGate(Vector2 position)
            : base(position, TextureRegistries.placeHolder, 10, 50) 
        { }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            RemoveFromSpatialHashing(scene);
            base.Update(gameTime, inputState, gameLayer, scene);
            AddToSpatialHashing(scene);

            var players = scene.GetObjectsInRadius<Player>(Position, (int)BoundedBox.Radius);
            if (!players.Any()) return;
            if (players.First().HyperDrive.TargetPlanetSystem is null) return;
            gameLayer.SwitchCurrentPlanetSystemScene(players.First().HyperDrive.TargetPlanetSystem);
            players.First().HyperDrive.TargetPlanetSystem = null;
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
