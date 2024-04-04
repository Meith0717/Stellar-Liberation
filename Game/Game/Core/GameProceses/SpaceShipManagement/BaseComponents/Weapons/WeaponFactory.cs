// WeaponFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.BaseComponents.Weapons
{
    public static class WeaponFactory
    {
        public static Weapon Get(Spaceship spaceship)
        {
            return new Weapon(Vector2.Zero, spaceship, GameSpriteRegistries.turette, GameSpriteRegistries.projectile, Color.Green, 1, 1, false);
        }
    }
}
