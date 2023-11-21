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
            switch (player.SensorArray.AimingShip)
            {
                case null:
                    scene.Camera2D.SetPosition(player.Position);
                    break;
                case not null:
                    var middleDistance = Vector2.Distance(player.Position, player.SensorArray.AimingShip.Position) / 2;
                    var directionToAimingShip = Vector2.Normalize(player.SensorArray.AimingShip.Position - player.Position);

                    var middlePosition = player.Position + (directionToAimingShip * middleDistance);
                    scene.Camera2D.MoveToTarget(middlePosition);
                    break;
            }
        }
    }
}
