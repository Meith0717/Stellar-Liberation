// Flagship.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.GameProceses;
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
    public class Flagship : Spacecraft, IGameObject, ICollidable
    {
        [JsonProperty] public HyperDrive HyperDrive { get; private set; }
        [JsonProperty] public ImpulseDrive ImpulseDrive { get; private set; }
        [JsonProperty] public Hangar Hangar { get; private set; }

        public Flagship(Vector2 position, Fractions fraction)
            : base(position, fraction, GameSpriteRegistries.destroyer, 5)
        {; }

        public void Populate(float shieldForcePerc, float hullForcePerc, float shieldRegPerc, float hullRegPerc, List<Weapon> weapons, float impulseVelocity, float hyperVelocity, int hangarCapacity)
        {
            Populate(shieldForcePerc, hullForcePerc, shieldRegPerc, hullRegPerc, weapons);
            HyperDrive = new(hyperVelocity);
            ImpulseDrive = new(impulseVelocity);
            Hangar = new(30);
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            HyperDrive.Update(gameTime, this, gameState, planetsystemState);
            ImpulseDrive.Move(gameTime, this, Defense.HullPercentage);
            base.Update(gameTime, gameState, planetsystemState);
            AttakEnemy(planetsystemState);
        }

        private void AttakEnemy(PlanetsystemState planetsystemState)
        {
            var target = Sensors.Opponents.FirstOrDefault(defaultValue: null);
            if (target is not null)
            {
                Hangar.Spawn(Position, planetsystemState);
                Weapons.AimTarget(target);
                if (Vector2.Distance(target.Position, Position) <= 50000)
                    Weapons.Fire();
                else
                    Weapons.StopFire();
                return;
            }
            Weapons.StopFire();
        }
    }
}