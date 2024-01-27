// Player.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses;
using System;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips
{
    [Serializable]
    public class Player : SpaceShip
    {
        public Player() : base(Vector2.Zero, new(
                    textureID: GameSpriteRegistries.destroyer,
                    textureScale: 1,
                    sensorRange: 5000,
                    velocity: 5,
                    turretCoolDown: 500,
                    shieldForce: 100,
                    hullForce: 100,
                    fraction: Fractions.Allied,
                    turretPositions: new()
                    {
                        new(110, 35),
                        new(110, -35),
                        new(-130, 100),
                        new(-130, -100),
                        new(-150, 0)
                    },
                    aiBehaviors: new()
                ))
        {; }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            SublightDrive.ControlByInput(this, inputState, scene.WorldMousePosition);
            WeaponSystem.AimShip(SensorSystem.GetAimingShip(Position));
            WeaponSystem.ControlByInput(inputState, scene.WorldMousePosition);
            base.Update(gameTime, inputState, scene);
        }
    }
}
