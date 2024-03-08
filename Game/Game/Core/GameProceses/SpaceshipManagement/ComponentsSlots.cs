// ComponentsSlots.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.


using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.Recources.Weapons;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.GameProceses.SpaceshipManagement
{
    public class ComponentsSlots
    {
        private readonly Spaceship mSpaceship;
        public readonly List<Weapon> Weapons;

        public ComponentsSlots(Spaceship spaceship)
        {
            mSpaceship = spaceship;
            Weapons = new() { WeaponFactory.Get(spaceship) };
        }

        public void FireWeapons(PlanetSystem planetSystem, Spaceship target)
        {
        }

        public void Update() 
        {
            foreach (var weapon in Weapons)
                weapon.Update(mSpaceship.Rotation);
        }

        public void Draw() { }
    }
}
