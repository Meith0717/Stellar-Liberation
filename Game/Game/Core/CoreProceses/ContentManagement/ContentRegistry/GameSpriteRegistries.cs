// GameSpriteRegistries.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry
{
    public class GameSpriteRegistries : Registries
    {
        public readonly static Registry arrow = new(@"textures\gameSprites", "arrow");

        private readonly static string animations = @"textures\gameSprites\animations\";
        public readonly static Registry explosion = new(animations, "explosion");

        private readonly static string background = @"textures\gameSprites\background";
        public readonly static Registry gameBackground = new(background, "gameBackground");
        public readonly static Registry mapBackground = new(background, "mapBackground");

        private readonly static string crosshair = @"textures\gameSprites\crosshair";
        public readonly static Registry selectCrosshait = new(crosshair, "selectCrosshait");

        private readonly static string parlaxBackground = @"textures\gameSprites\parlaxBackground";
        public readonly static Registry gameBackgroundParlax1 = new(parlaxBackground, "gameBackgroundParlax");
        public readonly static Registry gameBackgroundParlax2 = new(parlaxBackground, "gameBackgroundParlax2");
        public readonly static Registry gameBackgroundParlax3 = new(parlaxBackground, "gameBackgroundParlax3");
        public readonly static Registry gameBackgroundParlax4 = new(parlaxBackground, "gameBackgroundParlax4");

        private readonly static string textures = @"textures\gameSprites\gameObjects";
        public readonly static Registry particle = new(textures, "particle");
        public readonly static Registry placeHolder = new(textures, "placeHolder");
        public readonly static Registry radar = new(textures, "radar");
        public readonly static Registry container = new(textures, "container");

        private readonly static string planets = @"textures\gameSprites\gameobjects\astronomicalbodys\planets";
        public readonly static Registry cold = new(planets, "cold0");
        public readonly static Registry cold1 = new(planets, "cold1");
        public readonly static Registry cold2 = new(planets, "cold2");
        public readonly static Registry cold3 = new(planets, "cold3");
        public readonly static Registry dry = new(planets, "dry0");
        public readonly static Registry dry1 = new(planets, "dry1");
        public readonly static Registry dry2 = new(planets, "dry2");
        public readonly static Registry dry3 = new(planets, "dry3");
        public readonly static Registry gas = new(planets, "gas0");
        public readonly static Registry gas1 = new(planets, "gas1");
        public readonly static Registry gas2 = new(planets, "gas2");
        public readonly static Registry gas3 = new(planets, "gas3");
        public readonly static Registry stone = new(planets, "stone0");
        public readonly static Registry stone1 = new(planets, "stone1");
        public readonly static Registry stone2 = new(planets, "stone2");
        public readonly static Registry stone3 = new(planets, "stone3");
        public readonly static Registry terrestrial = new(planets, "terrestrial0");
        public readonly static Registry terrestrial1 = new(planets, "terrestrial1");
        public readonly static Registry terrestrial2 = new(planets, "terrestrial2");
        public readonly static Registry terrestrial3 = new(planets, "terrestrial3");
        public readonly static Registry tropical = new(planets, "tropical0");
        public readonly static Registry tropical1 = new(planets, "tropical1");
        public readonly static Registry tropical2 = new(planets, "tropical2");
        public readonly static Registry tropical3 = new(planets, "tropical3");
        public readonly static Registry warm = new(planets, "warm0");
        public readonly static Registry warm1 = new(planets, "warm1");
        public readonly static Registry warm2 = new(planets, "warm2");
        public readonly static Registry warm3 = new(planets, "warm3");
        public readonly static Registry planetShadow = new(planets, "planetShadow");

        private readonly static string stars = @"textures\gameSprites\gameobjects\astronomicalbodys\stars";
        public readonly static Registry starBH = new(stars, "BH");
        public readonly static Registry star = new(stars, "star");
        public readonly static Registry starLightAlpha = new(stars, "StarLightAlpha");

        private readonly static string asteroids = @"textures\gameSprites\gameobjects\astronomicalbodys\asteroids";
        public readonly static Registry asteroid1 = new(asteroids, "asteroid1");
        public readonly static Registry asteroid2 = new(asteroids, "asteroid2");
        public readonly static Registry asteroid3 = new(asteroids, "asteroid3");
        public readonly static Registry asteroid4 = new(asteroids, "asteroid4");
        public readonly static Registry asteroid5 = new(asteroids, "asteroid5");
        public readonly static Registry asteroid6 = new(asteroids, "asteroid6");
        public readonly static Registry asteroid7 = new(asteroids, "asteroid7");
        public readonly static Registry asteroid8 = new(asteroids, "asteroid8");
        public readonly static Registry asteroid9 = new(asteroids, "asteroid9");
        public readonly static Registry asteroid10 = new(asteroids, "asteroid10");
        public readonly static Registry asteroid11 = new(asteroids, "asteroid11");
        public readonly static Registry asteroid12 = new(asteroids, "asteroid12");
        public readonly static Registry asteroid13 = new(asteroids, "asteroid13");
        public readonly static Registry asteroid14 = new(asteroids, "asteroid14");
        public readonly static Registry asteroid15 = new(asteroids, "asteroid15");

        private readonly static string items = @"textures\gameSprites\gameobjects\items";
        public readonly static Registry titan = new(items, "titan");
        public readonly static Registry quantumCrystals = new(items, "quantumCrystals");
        public readonly static Registry platin = new(items, "platin");
        public readonly static Registry iron = new(items, "iron");
        public readonly static Registry gold = new(items, "gold");
        public readonly static Registry darkMatter = new(items, "darkMatter");

        private readonly static string spacecrafts = @"textures\gameSprites\gameobjects\spacecrafts";
        public readonly static Registry unkownObj = new(spacecrafts, "unkownObj");
        public readonly static Registry scienceStation = new(spacecrafts, "scienceStation");

        private readonly static string spaceShips = @"textures\gameSprites\gameobjects\spacecrafts\spaceShips";
        // cargo
        public readonly static Registry cargo = new(spaceShips, "cargo");
        public readonly static Registry cargoBorders = new(spaceShips, "cargoBorders");
        public readonly static Registry cargoFrame = new(spaceShips, "cargoFrame");
        public readonly static Registry cargoHull = new(spaceShips, "cargoHull");
        public readonly static Registry cargoShield = new(spaceShips, "cargoShield");
        public readonly static Registry cargoStructure = new(spaceShips, "cargoStructure");
        // fighter
        public readonly static Registry fighter = new(spaceShips, "fighter");
        public readonly static Registry fighterBorders = new(spaceShips, "fighterBorders");
        public readonly static Registry fighterFrame = new(spaceShips, "fighterFrame");
        public readonly static Registry fighterHull = new(spaceShips, "fighterHull");
        public readonly static Registry fighterShield = new(spaceShips, "fighterShield");
        public readonly static Registry fighterStructure = new(spaceShips, "fighterStructure");
        // bomber
        public readonly static Registry bomber = new(spaceShips, "bomber");
        public readonly static Registry bomberBorders = new(spaceShips, "bomberBorders");
        public readonly static Registry bomberFrame = new(spaceShips, "bomberFrame");
        public readonly static Registry bomberHull = new(spaceShips, "bomberHull");
        public readonly static Registry bomberShield = new(spaceShips, "bomberShield");
        public readonly static Registry bomberStructure = new(spaceShips, "bomberStructure");
        // interceptor
        public readonly static Registry interceptor = new(spaceShips, "interceptor");
        public readonly static Registry interceptorBorders = new(spaceShips, "interceptorBorders");
        public readonly static Registry interceptorFrame = new(spaceShips, "interceptorFrame");
        public readonly static Registry interceptorHull = new(spaceShips, "interceptorHull");
        public readonly static Registry interceptorShield = new(spaceShips, "interceptorShield");
        public readonly static Registry interceptorStructure = new(spaceShips, "interceptorStructure");
        // destroyer
        public readonly static Registry destroyer = new(spaceShips, "destroyer");
        public readonly static Registry destroyerBorders = new(spaceShips, "destroyerBorders");
        public readonly static Registry destroyerFrame = new(spaceShips, "destroyerFrame");
        public readonly static Registry destroyerHull = new(spaceShips, "destroyerHull");
        public readonly static Registry destroyerShield = new(spaceShips, "destroyerShield");
        public readonly static Registry destroyerStructure = new(spaceShips, "destroyerStructure");
        // cruiser
        public readonly static Registry cruiser = new(spaceShips, "cruiser");
        public readonly static Registry cruiserBorders = new(spaceShips, "cruiserBorders");
        public readonly static Registry cruiserFrame = new(spaceShips, "cruiserFrame");
        public readonly static Registry cruiserHull = new(spaceShips, "cruiserHull");
        public readonly static Registry cruiserShield = new(spaceShips, "cruiserShield");
        public readonly static Registry cruiserStructure = new(spaceShips, "cruiserStructure");

        private readonly static string weapons = @"textures\gameSprites\gameobjects\weapons";
        public readonly static Registry projectile = new(weapons, "projectile");
        public readonly static Registry turette = new(weapons, "turette");
    }
}
