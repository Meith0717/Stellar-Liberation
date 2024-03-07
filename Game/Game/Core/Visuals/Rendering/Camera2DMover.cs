// Camera2DMover.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;

namespace StellarLiberation.Game.Core.Visuals.Rendering
{
    public static class Camera2DMover
    {
        public static void ControllZoom(GameTime gameTime, InputState inputState, Camera2D camera2D, float minZoom, float maxZoom)
        {
            var zoom = 0f;
            var multiplier = 1;

            inputState.DoAction(ActionType.CameraZoomIn, () => zoom += 100f / (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            inputState.DoAction(ActionType.CameraZoomOut, () => zoom -= 100f / (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            inputState.DoAction(ActionType.CtrlLeft, () => multiplier = 5);

            camera2D.Zoom *= 1 + (zoom * multiplier) * (float)(0.001 * gameTime.ElapsedGameTime.TotalMilliseconds);
            camera2D.Zoom = MathHelper.Clamp(camera2D.Zoom, minZoom, maxZoom);
        }

        private static Vector2 lastMousePosition;

        public static bool UpdateCameraByMouseDrag(InputState inputState, Camera2D camera)
        {
            var wasMoved = false;
            if (inputState.HasAction(ActionType.LeftClickHold))
            {
                Vector2 delta = inputState.mMousePosition - lastMousePosition;
                camera.Position -= delta / camera.Zoom;
                wasMoved = true;
            }

            lastMousePosition = inputState.mMousePosition;
            return wasMoved;
        }

        public static bool MoveByKeys(GameTime gameTime, InputState inputState, Camera2D camera)
        {
            var x = 0f;
            var y = 0f;
            inputState.DoAction(ActionType.MoveCameraLeft, () => x -= (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            inputState.DoAction(ActionType.MoveCameraRight, () => x += (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            inputState.DoAction(ActionType.MoveCameraUp, () => y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            inputState.DoAction(ActionType.MoveCameraDown, () => y += (float)gameTime.ElapsedGameTime.TotalMilliseconds);

            if (x == 0 && y == 0) return false;

            camera.Position.X += x / camera.Zoom;
            camera.Position.Y += y / camera.Zoom;
            return true;
        }
    }
}
