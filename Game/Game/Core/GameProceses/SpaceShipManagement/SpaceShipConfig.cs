// SpaceShipConfig.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses.AI;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement
{
    public readonly struct SpaceShipConfig
    {
        public readonly string TextureID;
        public readonly float TextureScale;
        public readonly int SensorRange;
        public readonly int Velocity;
        public readonly int TurretCoolDown;
        public readonly int ShieldForce;
        public readonly int HullForce;
        public readonly Fractions Fraction;
        public readonly List<Vector2> WeaponsPositions;
        public readonly List<Behavior> AIBehaviors;

        public SpaceShipConfig(
        string textureID,
        float textureScale,
        int sensorRange,
        int velocity,
        int turretCoolDown,
        int shieldForce,
        int hullForce,
        Fractions fraction,
        List<Vector2> turretPositions,
        List<Behavior> aiBehaviors)
        {
            TextureID = textureID;
            TextureScale = textureScale;
            SensorRange = sensorRange;
            Velocity = velocity;
            TurretCoolDown = turretCoolDown;
            ShieldForce = shieldForce;
            HullForce = hullForce;
            Fraction = fraction;
            WeaponsPositions = turretPositions;
            AIBehaviors = aiBehaviors;
        }
    }
}
