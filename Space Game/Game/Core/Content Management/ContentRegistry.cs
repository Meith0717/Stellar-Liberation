using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CelestialOdyssey.GameEngine.Content_Management
{
    public class Registry
    {
        public string Name;
        public string FilePath;

        public Registry(string dirPath, string fileName)
        {
            FilePath = Path.Combine(dirPath, fileName);
            Name = fileName;
        }

        public static implicit operator string(Registry registry)
        {
            return registry.Name;
        }
    }

    public  class ContentRegistry
    {
        private readonly static string background = @"textures\background";
        public readonly static Registry gameBackground = new(background, "gameBackground");
        public readonly static Registry gameBackgroundParlax = new(background, "gameBackgroundParlax");
        public readonly static Registry gameBackgroundParlax1 = new(background, "gameBackgroundParlax2");
        public readonly static Registry gameBackgroundParlax2 = new(background, "gameBackgroundParlax3");
        public readonly static Registry gameBackgroundParlax3 = new(background, "gameBackgroundParlax4");

        private readonly static string planets = @"textures\gameobjects\astronomicalbodys\planets";
        public readonly static Registry cold1 = new(planets, "cold1");
        public readonly static Registry cold2 = new(planets, "cold2");
        public readonly static Registry cold3 = new(planets, "cold3");
        public readonly static Registry cold4 = new(planets, "cold4");
        public readonly static Registry dry1 = new(planets, "dry1");
        public readonly static Registry dry2 = new(planets, "dry2");
        public readonly static Registry dry3 = new(planets, "dry3");
        public readonly static Registry dry4 = new(planets, "dry4");
        public readonly static Registry dry5 = new(planets, "dry5");
        public readonly static Registry dry6 = new(planets, "dry6");
        public readonly static Registry gas1 = new(planets, "gas1");
        public readonly static Registry gas2 = new(planets, "gas2");
        public readonly static Registry gas3 = new(planets, "gas3");
        public readonly static Registry gas4 = new(planets, "gas4");
        public readonly static Registry stone1 = new(planets, "stone1");
        public readonly static Registry stone2 = new(planets, "stone2");
        public readonly static Registry stone3 = new(planets, "stone3");
        public readonly static Registry stone4 = new(planets, "stone4");
        public readonly static Registry stone5 = new(planets, "stone5");
        public readonly static Registry stone6 = new(planets, "stone6");
        public readonly static Registry terrestrial1 = new(planets, "terrestrial1");
        public readonly static Registry terrestrial2 = new(planets, "terrestrial2");
        public readonly static Registry terrestrial3 = new(planets, "terrestrial3");
        public readonly static Registry terrestrial4 = new(planets, "terrestrial4");
        public readonly static Registry terrestrial5 = new(planets, "terrestrial5");
        public readonly static Registry terrestrial6 = new(planets, "terrestrial6");
        public readonly static Registry terrestrial7 = new(planets, "terrestrial7");
        public readonly static Registry terrestrial8 = new(planets, "terrestrial8");
        public readonly static Registry warm1 = new(planets, "warm1");
        public readonly static Registry warm2 = new(planets, "warm2");
        public readonly static Registry warm3 = new(planets, "warm3");
        public readonly static Registry warm4 = new(planets, "warm4");
        public readonly static Registry planetShadow = new(planets, "planetShadow");

        private readonly static string stars = @"textures\gameobjects\astronomicalbodys\stars";
        public readonly static Registry starA = new(stars, "A");
        public readonly static Registry starB = new(stars, "B");
        public readonly static Registry starF = new(stars, "F");
        public readonly static Registry starG = new(stars, "G");
        public readonly static Registry starK = new(stars, "K");
        public readonly static Registry starM = new(stars, "M");
        public readonly static Registry starO = new(stars, "O");
        public readonly static Registry starBH = new(stars, "BH");
        public readonly static Registry starLightAlpha = new(stars, "StarLightAlpha");

        private readonly static string items = @"textures\gameobjects\items";
        public readonly static Registry odyssyum = new(items, "odyssyum");
        public readonly static Registry postyum = new(items, "postyum");
        public readonly static Registry metall = new(items, "metall");

        private readonly static string spacecrafts = @"textures\gameobjects\spacecrafts";
        public readonly static Registry ship = new(spacecrafts, "ship");
        public readonly static Registry pirate = new(spacecrafts, "pirate");

        private readonly static string weapons = @"textures\gameobjects\weapons";
        public readonly static Registry photonTorpedo = new(weapons, "photonTorpedo");

        private readonly static string textures = @"textures\";
        public readonly static Registry pixle = new(textures, "pixle");
        public readonly static Registry cursor = new(textures, "cursor");
        public readonly static Registry cursor1 = new(textures, "cursor1");

        private readonly static string crosshair = @"textures\crosshair";
        public readonly static Registry mapCrosshair = new(crosshair, "selectCrosshait");
        public readonly static Registry dot = new(crosshair, "dot");

        private readonly static string layer1 = @"textures\UserInterface\Layer";
        public readonly static Registry layer = new(layer1, "layer");
        public readonly static Registry circle = new(layer1, "circle");

        private readonly static string pauseLayer = @"textures\UserInterface\PauseLayer";
        public readonly static Registry buttonExitgame = new(pauseLayer, "buttonExitgame");
        public readonly static Registry buttonContinue = new(pauseLayer, "buttonContinue");

        private readonly static string soundEffects = @"SoundEffects\";
        public readonly static Registry torpedoHit = new(soundEffects, "torpedoHit");
        public readonly static Registry torpedoFire = new(soundEffects, "torpedoFire");
        public readonly static Registry collect = new(soundEffects, "collect");
        public readonly static Registry bgMusicGame = new(soundEffects, "bgMusicGame");

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
