// Battleship.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.AI;
using StellarLiberation.Game.Core.GameProceses.AI.Behaviors;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons;
using StellarLiberation.Game.Layers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public class Battleship : Spacecraft, IGameObject, ICollidable
    {
        [JsonProperty] public readonly BattleshipID BattleshipID;
        [JsonProperty] public ImpulseDrive ImpulseDrive { get; private set; }

        public Battleship(Vector2 position, Fractions fraction, string textureID, float textureScale, Vector2 engineTrailPosition)
            : base(position, fraction, textureID, textureScale, engineTrailPosition)
        {; }

        public void Populate(float shieldForcePerc, float hullForcePerc, float shieldRegPerc, float hullRegPerc, List<Weapon> weapons, float impulseVelocity)
        {
            Populate(shieldForcePerc, hullForcePerc, shieldRegPerc, hullRegPerc, weapons);
            ImpulseDrive = new(impulseVelocity);
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            ImpulseDrive.Move(gameTime, this, Defense.HullPercentage);
            base.Update(gameTime, gameState, planetsystemState);
            AttakEnemy();
        }

        private void AttakEnemy()
        {
            var target = Sensors.Opponents.FirstOrDefault(defaultValue: null);
            if (target is null) return;
            Weapons.AimTarget(target);
        }


    }
}
