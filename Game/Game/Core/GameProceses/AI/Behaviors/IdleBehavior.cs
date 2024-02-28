// IdleBehavior.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarLiberation.Game.Core.GameProceses.AI.Behaviors
{
    internal class IdleBehavior : Behavior
    {
        public override void Execute(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            spaceShip.SublightDrive.SetVelocity(0);
        }

        public override double GetScore(GameTime gameTime, SpaceShip spaceShip, GameLayer scene)
        {
            return 0.01f;
        }

        public override void Reset(SpaceShip spaceShip) { }
    }
}
