// CameraController.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.GameObjects;

namespace StellarLiberation.Game.Core.Camera
{
    public static class Camera2DController
    {
        public static void Track(Player player, Scene scene)
        {
            var camera = scene.Camera2D;
            camera.SetPosition(player.Position);
            return;
        }
    }
}
