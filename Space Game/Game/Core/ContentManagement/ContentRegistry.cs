// ContentRegistry.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CelestialOdyssey.GameEngine.Content_Management
{
    public enum RegistryType { Texture, Font, Sound }

    public class Registry
    {
        public string Name;
        public string FilePath;
        public RegistryType RegistryType;

        public Registry(string dirPath, string fileName, RegistryType type)
        {
            FilePath = Path.Combine(dirPath, fileName);
            Name = fileName;
            RegistryType = type;
        }

        public static implicit operator string(Registry registry)
        {
            return registry.Name;
        }
    }

    public class ContentRegistry
    {
        private readonly static string background = @"textures\background";
        public readonly static Registry gameBackground = new(background, "gameBackground", RegistryType.Texture);
        public readonly static Registry mapBackground = new(background, "mapBackground", RegistryType.Texture);
        public readonly static Registry gameBackgroundParlax1 = new(background, "gameBackgroundParlax", RegistryType.Texture);
        public readonly static Registry gameBackgroundParlax2 = new(background, "gameBackgroundParlax2", RegistryType.Texture);

        private readonly static string planets = @"textures\gameobjects\astronomicalbodys\planets";
        public readonly static Registry cold1 = new(planets, "cold1", RegistryType.Texture);
        public readonly static Registry cold2 = new(planets, "cold2", RegistryType.Texture);
        public readonly static Registry cold3 = new(planets, "cold3", RegistryType.Texture);
        public readonly static Registry cold4 = new(planets, "cold4", RegistryType.Texture);
        public readonly static Registry dry1 = new(planets, "dry1", RegistryType.Texture);
        public readonly static Registry dry2 = new(planets, "dry2", RegistryType.Texture);
        public readonly static Registry dry3 = new(planets, "dry3", RegistryType.Texture);
        public readonly static Registry dry4 = new(planets, "dry4", RegistryType.Texture);
        public readonly static Registry dry5 = new(planets, "dry5", RegistryType.Texture);
        public readonly static Registry dry6 = new(planets, "dry6", RegistryType.Texture);
        public readonly static Registry gas1 = new(planets, "gas1", RegistryType.Texture);
        public readonly static Registry gas2 = new(planets, "gas2", RegistryType.Texture);
        public readonly static Registry gas3 = new(planets, "gas3", RegistryType.Texture);
        public readonly static Registry gas4 = new(planets, "gas4", RegistryType.Texture);
        public readonly static Registry stone1 = new(planets, "stone1", RegistryType.Texture);
        public readonly static Registry stone2 = new(planets, "stone2", RegistryType.Texture);
        public readonly static Registry stone3 = new(planets, "stone3", RegistryType.Texture);
        public readonly static Registry stone4 = new(planets, "stone4", RegistryType.Texture);
        public readonly static Registry stone5 = new(planets, "stone5", RegistryType.Texture);
        public readonly static Registry stone6 = new(planets, "stone6", RegistryType.Texture);
        public readonly static Registry terrestrial1 = new(planets, "terrestrial1", RegistryType.Texture);
        public readonly static Registry terrestrial2 = new(planets, "terrestrial2", RegistryType.Texture);
        public readonly static Registry terrestrial3 = new(planets, "terrestrial3", RegistryType.Texture);
        public readonly static Registry terrestrial4 = new(planets, "terrestrial4", RegistryType.Texture);
        public readonly static Registry terrestrial5 = new(planets, "terrestrial5", RegistryType.Texture);
        public readonly static Registry terrestrial6 = new(planets, "terrestrial6", RegistryType.Texture);
        public readonly static Registry terrestrial7 = new(planets, "terrestrial7", RegistryType.Texture);
        public readonly static Registry terrestrial8 = new(planets, "terrestrial8", RegistryType.Texture);
        public readonly static Registry warm1 = new(planets, "warm1", RegistryType.Texture);
        public readonly static Registry warm2 = new(planets, "warm2", RegistryType.Texture);
        public readonly static Registry warm3 = new(planets, "warm3", RegistryType.Texture);
        public readonly static Registry warm4 = new(planets, "warm4", RegistryType.Texture);
        public readonly static Registry planetShadow = new(planets, "planetShadow", RegistryType.Texture);

        private readonly static string stars = @"textures\gameobjects\astronomicalbodys\stars";
        public readonly static Registry starA = new(stars, "A", RegistryType.Texture);
        public readonly static Registry starB = new(stars, "B", RegistryType.Texture);
        public readonly static Registry starF = new(stars, "F", RegistryType.Texture);
        public readonly static Registry starG = new(stars, "G", RegistryType.Texture);
        public readonly static Registry starK = new(stars, "K", RegistryType.Texture);
        public readonly static Registry starM = new(stars, "M", RegistryType.Texture);
        public readonly static Registry starO = new(stars, "O", RegistryType.Texture);
        public readonly static Registry starBH = new(stars, "BH", RegistryType.Texture);
        public readonly static Registry starLightAlpha = new(stars, "StarLightAlpha", RegistryType.Texture);

        private readonly static string items = @"textures\gameobjects\items";
        public readonly static Registry odyssyum = new(items, "odyssyum", RegistryType.Texture);
        public readonly static Registry postyum = new(items, "postyum", RegistryType.Texture);
        public readonly static Registry metall = new(items, "metall", RegistryType.Texture);

        private readonly static string spacecrafts = @"textures\gameobjects\spacecrafts";
        public readonly static Registry player = new(spacecrafts, "player", RegistryType.Texture);
        public readonly static Registry playerShield = new(spacecrafts, "playerShield", RegistryType.Texture);
        public readonly static Registry enemyFighter = new(spacecrafts, "enemyFighter", RegistryType.Texture);
        public readonly static Registry enemyFighterShield = new(spacecrafts, "enemyFighterShield", RegistryType.Texture);
        public readonly static Registry enemyCorvette = new(spacecrafts, "enemyCorvette", RegistryType.Texture);
        public readonly static Registry enemyCorvetteShield = new(spacecrafts, "enemyCorvetteShield", RegistryType.Texture);
        public readonly static Registry enemyBattleShip = new(spacecrafts, "enemyBattleShip", RegistryType.Texture);
        public readonly static Registry enemyBattleShipShield = new(spacecrafts, "enemyBattleShipShield", RegistryType.Texture);

        private readonly static string weapons = @"textures\gameobjects\weapons";
        public readonly static Registry projectile = new(weapons, "projectile", RegistryType.Texture);
        public readonly static Registry turette = new(weapons, "turette", RegistryType.Texture);

        private readonly static string textures = @"textures\";
        public readonly static Registry pixle = new(textures, "pixle", RegistryType.Texture);
        public readonly static Registry cursor = new(textures, "cursor", RegistryType.Texture);
        public readonly static Registry cursor1 = new(textures, "cursor1", RegistryType.Texture);


        private readonly static string crosshair = @"textures\crosshair";
        public readonly static Registry mapCrosshair = new(crosshair, "selectCrosshait", RegistryType.Texture);
        public readonly static Registry dot = new(crosshair, "dot", RegistryType.Texture);

        private readonly static string layer1 = @"textures\UserInterface\Layer";
        public readonly static Registry layer = new(layer1, "layer", RegistryType.Texture);
        public readonly static Registry circle = new(layer1, "circle", RegistryType.Texture);

        private readonly static string pauseLayer = @"textures\UserInterface\PauseLayer";
        public readonly static Registry buttonExitgame = new(pauseLayer, "buttonExitgame", RegistryType.Texture);
        public readonly static Registry buttonContinue = new(pauseLayer, "buttonContinue", RegistryType.Texture);

        private readonly static string soundEffects = @"SoundEffects\";
        public readonly static Registry torpedoHit = new(soundEffects, "torpedoHit", RegistryType.Sound);
        public readonly static Registry torpedoFire = new(soundEffects, "torpedoFire", RegistryType.Sound);
        public readonly static Registry collect = new(soundEffects, "collect", RegistryType.Sound);
        public readonly static Registry bgMusicGame = new(soundEffects, "bgMusicGame", RegistryType.Sound);
        public readonly static Registry ChargeHyperdrive = new(soundEffects, "chargeHyperdrive", RegistryType.Sound);
        public readonly static Registry CoolHyperdrive = new(soundEffects, "coolHyperdrive", RegistryType.Sound);

        private readonly static string animations = @"textures\animations\";
        public readonly static Registry explosion = new(animations, "explosion", RegistryType.Texture);

        public static List<Registry> IterateThroughRegistries()
        {
            FieldInfo[] fields = typeof(ContentRegistry).GetFields(BindingFlags.Public | BindingFlags.Static);
            List<Registry> registrys = new();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(Registry))
                {
                    Registry registry = (Registry)field.GetValue(null);
                    registrys.Add(registry);
                }
            }
            return registrys;
        }
    }
}
