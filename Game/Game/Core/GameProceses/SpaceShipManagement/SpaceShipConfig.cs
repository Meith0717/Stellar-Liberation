// SpaceshipConfig.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.AI;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement
{
    public readonly struct SpaceshipConfig
    {
        public readonly string TextureID;
        public readonly float TextureScale;
        public readonly int Velocity;
        public readonly int TurretCoolDown;
        public readonly int ShieldForce;
        public readonly int HullForce;
        public readonly List<Vector2> WeaponsPositions;

        public SpaceshipConfig(
        string textureID,
        float textureScale,
        int velocity,
        int turretCoolDown,
        int shieldForce,
        int hullForce,
        List<Vector2> turretPositions)
        {
            TextureID = textureID;
            TextureScale = textureScale;
            Velocity = velocity;
            TurretCoolDown = turretCoolDown;
            ShieldForce = shieldForce;
            HullForce = hullForce;
            WeaponsPositions = turretPositions;
        }
    }
}
