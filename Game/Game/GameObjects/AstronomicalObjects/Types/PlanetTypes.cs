// PlanetTypes.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    internal static class PlanetTypes
    {
        public static Planet GetPlanet(Vector2 orbitCenter, int oribitRadius) => new(orbitCenter, oribitRadius, GameSpriteRegistries.terrestrial1, 10);
    }
}
