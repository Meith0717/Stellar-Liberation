// Camera2DController.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Allies;

namespace StellarLiberation.Game.Core.Visuals.Rendering
{
    public static class Camera2DController
    {
        private static bool mTrack;

        public static void Manage(Player player, InputState inputState, Camera2D camera2D)
        {
            if (Camera2DMover.UpdateCameraByMouseDrag(inputState, camera2D) || Camera2DMover.MoveByKeys(inputState, camera2D)) mTrack = false;
            inputState.DoAction(ActionType.ToggleCameraMode, () => mTrack = true);

            if (mTrack) { camera2D.Position = player.Position; }
        }
    }
}
