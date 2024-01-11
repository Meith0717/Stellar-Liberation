// TextureRegistries.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry
{
    public class GameSpriteRegistries : Registries
    {

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

        private readonly static string planets = @"textures\gameSprites\gameobjects\astronomicalbodys\planets";
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

        private readonly static string stars = @"textures\gameSprites\gameobjects\astronomicalbodys\stars";
        public readonly static Registry starA = new(stars, "A");
        public readonly static Registry starB = new(stars, "B");
        public readonly static Registry starF = new(stars, "F");
        public readonly static Registry starG = new(stars, "G");
        public readonly static Registry starK = new(stars, "K");
        public readonly static Registry starM = new(stars, "M");
        public readonly static Registry starO = new(stars, "O");
        public readonly static Registry starBH = new(stars, "BH");
        public readonly static Registry starLightAlpha = new(stars, "StarLightAlpha");
        public readonly static Registry star = new(stars, "star");

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
        public readonly static Registry player = new(spacecrafts, "player");
        public readonly static Registry playerShield = new(spacecrafts, "playerShield");
        public readonly static Registry enemyFighter = new(spacecrafts, "enemyFighter");
        public readonly static Registry enemyFighterShield = new(spacecrafts, "enemyFighterShield");
        public readonly static Registry enemyBomber = new(spacecrafts, "enemyBomber");
        public readonly static Registry enemyBomberShield = new(spacecrafts, "enemyBomberShield");
        public readonly static Registry enemyBattleShip = new(spacecrafts, "enemyBattleShip");
        public readonly static Registry enemyBattleShipShield = new(spacecrafts, "enemyBattleShipShield");
        public readonly static Registry enemyCarrior = new(spacecrafts, "enemyCarrior");
        public readonly static Registry enemyCarriorShield = new(spacecrafts, "enemyCarriorShield");
        public readonly static Registry unkownObj = new(spacecrafts, "unkownObj");
        public readonly static Registry scienceStation = new(spacecrafts, "scienceStation");

        private readonly static string weapons = @"textures\gameSprites\gameobjects\weapons";
        public readonly static Registry projectile = new(weapons, "projectile");
        public readonly static Registry turette = new(weapons, "turette");
    }
}
