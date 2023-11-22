// CameraController.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.GameObjects;

namespace StellarLiberation.Game.Core.Camera
{
    public static class Camer2DController
    {
        public static void Track(Player player, Scene scene)
        {
            var camera = scene.Camera2D;
            var aimingShip = player.SensorArray.AimingShip;

            switch (player.SensorArray.AimingShip)
            {
                case null:
                    camera.SetPosition(player.Position);
                    break;
                case not null:
                    var middleDistance = Vector2.Distance(player.Position, aimingShip.Position) / 2;
                    var directionToAimingShip = Vector2.Normalize(aimingShip.Position - player.Position);

                    var middlePosition = player.Position + (directionToAimingShip * middleDistance);
                    camera.MoveToTarget(middlePosition);
                    break;
            }
        }
    }
}
