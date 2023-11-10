// QuantumGate.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using System.Linq;

namespace StellarLiberation.Game.GameObjects
{
    public class QuantumGate : GameObject
    {

        public QuantumGate(Vector2 position)
            : base(position, TextureRegistries.placeHolder, 10, 50) 
        { }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            RemoveFromSpatialHashing(scene);
            base.Update(gameTime, inputState, scene);
            AddToSpatialHashing(scene);

            var players = scene.GetObjectsInRadius<Player>(Position, (int)BoundedBox.Radius);
            if (!players.Any()) return;
            if (players.First().HyperDrive.TargetPlanetSystem is null) return;
            scene.GameLayer.SwitchCurrentPlanetSystemScene(players.First().HyperDrive.TargetPlanetSystem);
            players.First().HyperDrive.TargetPlanetSystem = null;
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
