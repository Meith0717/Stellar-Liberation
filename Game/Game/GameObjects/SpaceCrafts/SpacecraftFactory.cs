// SpacecraftFactory.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.Spacecrafts
{
    public enum BattleshipID { BomberMKI, InterceptorMKI, FighterMKI, BomberMKII, InterceptorMKII, FighterMKII }
    public enum FlagshipID { }
    public enum SpaceStationID { }

    public class SpacecraftFactory
    {
        public static Battleship GetBattleship(Vector2 position, BattleshipID shipID, Fractions fraction)
        {
            Battleship spaceship = shipID switch
            {
                BattleshipID.BomberMKI => new(shipID, position, fraction, "bomber", .25f, new(-60, 0)),
                BattleshipID.BomberMKII => new(shipID, position, fraction, "bomber", .25f, new(-60, 0)),
                BattleshipID.InterceptorMKI => new(shipID, position, fraction, "destroyer", .25f, new(-60, 0)),
                BattleshipID.InterceptorMKII => new(shipID, position, fraction, "destroyer", .25f, new(-60, 0)),
                BattleshipID.FighterMKI => new(shipID, position, fraction, "fighter", .25f, new(-60, 0)),
                BattleshipID.FighterMKII => new(shipID, position, fraction, "fighter", .25f, new(-60, 0)),
                _ => throw new System.NotImplementedException()
            };
            var weapons = new List<Weapon>()
            {
                WeaponFactory.Instance.GetWeapon(Vector2.Zero, "Phaser I")
            };
            spaceship.Populate(500, 500, 0, 0, weapons, 1);
            return spaceship;
        }

        public static Flagship GetFlagship(Vector2 position, Fractions fraction)
        {
            Flagship spaceship = new(position, fraction, new(-750, 0));
            var weapons = new List<Weapon>()
            {
                WeaponFactory.Instance.GetWeapon(new(-290, 610), "Phaser I"),
                WeaponFactory.Instance.GetWeapon(new(-290, -610), "Phaser I")
            };
            spaceship.Populate(5000, 5000, 1, 0, weapons, 1, 1, 10);
            return spaceship;
        }

        private static Color GetWeaponColor(Fractions fraction)
            => fraction switch { Fractions.Neutral => Color.White, Fractions.Allied => Color.LightBlue, Fractions.Enemys => Color.MonoGameOrange };
    }
}