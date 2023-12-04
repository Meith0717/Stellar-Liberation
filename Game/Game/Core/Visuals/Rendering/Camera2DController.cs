// Camera2DController.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.GameObjects.SpaceShipManagement;

namespace StellarLiberation.Game.Core.Visuals.Rendering
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
