// QuantumGate.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Core.GameEngine.Content_Management;
using StellarLiberation.Game.Core.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;

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

            var player = scene.GameLayer.Player;

            if (!BoundedBox.Intersects(player.BoundedBox)) return;
            if (player.HyperDrive.TargetPlanetSystem is null) return;
            scene.GameLayer.CurrentSystem = player.HyperDrive.TargetPlanetSystem;
            scene.GameLayer.Player.Position = player.HyperDrive.TargetPlanetSystem.QuantumGate.Position;
            player.HyperDrive.TargetPlanetSystem = null;
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
