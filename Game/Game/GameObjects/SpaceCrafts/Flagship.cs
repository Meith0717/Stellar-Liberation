// Flagship.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons;
using StellarLiberation.Game.Layers;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public class Flagship : Spacecraft, IGameObject, ICollidable
    {
        [JsonProperty] public readonly NavigationSystem NavigationSystem;
        [JsonProperty] public HyperDrive HyperDrive { get; private set; }
        [JsonProperty] public ImpulseDrive ImpulseDrive { get; private set; }
        [JsonProperty] public Hangar Hangar { get; private set; }

        public Flagship(Vector2 position, Fractions fraction, Vector2 engineTrailPosition)
            : base(position, fraction, "destroyer", 5, engineTrailPosition)
        {
            NavigationSystem = new();
        }

        public void Populate(float shieldForcePerc, float hullForcePerc, float shieldRegPerc, float hullRegPerc, List<Weapon> weapons, float impulseVelocity, float hyperVelocity, int hangarCapacity)
        {
            Populate(shieldForcePerc, hullForcePerc, shieldRegPerc, hullRegPerc, weapons);
            HyperDrive = new(hyperVelocity);
            ImpulseDrive = new(impulseVelocity);
            Hangar = new(hangarCapacity = 30);
            Hangar.AssignNewShip(BattleshipID.InterceptorMKI, 10);
            Hangar.AssignNewShip(BattleshipID.FighterMKI, 10);
            Hangar.AssignNewShip(BattleshipID.BomberMKI, 10);
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            HyperDrive.Update(gameTime, this, gameState, planetsystemState);
            ImpulseDrive.Move(gameTime, this, Defense.HullPercentage);
            NavigationSystem.Update(this, ImpulseDrive, HyperDrive, planetsystemState);
            base.Update(gameTime, gameState, planetsystemState);
            Hangar.Update(this, planetsystemState);
        }
    }
}