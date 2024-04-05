// Battleship.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipComponents;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Layers;
using System;
using StellarLiberation.Game.Core.GameProceses.SpaceShipComponents.Weapons;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public class Battleship : Spacecraft, IGameObject, ICollidable
    {
        [JsonProperty] public HyperDrive HyperDrive { get; private set; }
        [JsonProperty] public ImpulseDrive ImpulseDrive { get; private set; }

        public Battleship(Vector2 position, Fractions fraction, string textureID, float textureScale)
            : base(position, fraction, textureID, textureScale)
        {; }

        public void Populate(float shieldForcePerc, float hullForcePerc, float shieldRegPerc, float hullRegPerc, List<Weapon> weapons, float impulseVelocityPerc, float hyperVelocityPerc)
        {
            Populate(shieldForcePerc, hullForcePerc, shieldRegPerc, hullRegPerc, weapons);
            HyperDrive = new(impulseVelocityPerc);
            ImpulseDrive = new(hyperVelocityPerc);
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            HyperDrive.Move(gameTime, this, planetsystemState);
            ImpulseDrive.Move(gameTime, this, Defense.HullPercentage);
            base.Update(gameTime, gameState, planetsystemState);
        }
    }
}
