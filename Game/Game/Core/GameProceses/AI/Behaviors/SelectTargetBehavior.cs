// SelectTargetBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    public class SelectTargetBehavior : Behavior
    {

        public override void Execute(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            spaceShip.SublightDrive.SetVelocity(0);
            spaceShip.SensorSystem.GetAimingShip(spaceShip.Position);
        }
         
        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, GameLayer scene) => MathHelper.Min(spaceShip.SensorSystem.OpponentsInRannge.Count, 1) * .5f;

        public override void Reset(SpaceShip spaceShip) { }
    }
}
