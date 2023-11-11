// TextureRegistry.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

namespace StellarLiberation.Game.Core.ContentManagement.ContentRegistry
{
    public class TextureRegistries : Registries
    {
        private readonly static string background = @"textures\background";
        public readonly static Registry gameBackground = new(background, "gameBackground");
        public readonly static Registry mapBackground = new(background, "mapBackground");
        public readonly static Registry gameBackgroundParlax1 = new(background, "gameBackgroundParlax");
        public readonly static Registry gameBackgroundParlax2 = new(background, "gameBackgroundParlax2");

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

        private readonly static string asteroids = @"textures\gameobjects\astronomicalbodys\asteroids";
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

        private readonly static string items = @"textures\gameobjects\items";
        public readonly static Registry odyssyum = new(items, "odyssyum");
        public readonly static Registry postyum = new(items, "postyum");
        public readonly static Registry metall = new(items, "metall");

        private readonly static string spacecrafts = @"textures\gameobjects\spacecrafts";
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

        private readonly static string weapons = @"textures\gameobjects\weapons";
        public readonly static Registry projectile = new(weapons, "projectile");
        public readonly static Registry turette = new(weapons, "turette");

        private readonly static string textures = @"textures\";
        public readonly static Registry pixle = new(textures, "pixle");
        public readonly static Registry cursor = new(textures, "cursor");
        public readonly static Registry placeHolder = new(textures, "placeHolder");
        public readonly static Registry title = new(textures, "title");
        public readonly static Registry menueBackground = new(textures, "menueBackground");

        private readonly static string crosshair = @"textures\crosshair";
        public readonly static Registry mapCrosshair = new(crosshair, "selectCrosshait");
        public readonly static Registry dot = new(crosshair, "dot");

        private readonly static string _layer = @"textures\UserInterface\layer";
        public readonly static Registry layer = new(_layer, "layer");
        public readonly static Registry layer1 = new(_layer, "layer1");
        public readonly static Registry circle = new(_layer, "circle");

        private readonly static string menueButtons = @"textures\UserInterface\buttons";
        public readonly static Registry button = new(menueButtons, "button");
        public readonly static Registry pauseButton = new(menueButtons, "pauseButton");
        public readonly static Registry copyrightButton = new(menueButtons, "copyrightButton");

        private readonly static string animations = @"textures\animations\";
        public readonly static Registry explosion = new(animations, "explosion");

        private readonly static string bar = @"textures\UserInterface\bar";
        public readonly static Registry barHorizontalLeft = new(bar, "barHorizontalLeft");
        public readonly static Registry barHorizontalMid = new(bar, "barHorizontalMid");
        public readonly static Registry barHorizontalRight = new(bar, "barHorizontalRight");
        public readonly static Registry barHorizontalShadowLeft = new(bar, "barHorizontalShadowLeft");
        public readonly static Registry barHorizontalShadowMid = new(bar, "barHorizontalShadowMid");
        public readonly static Registry barHorizontalShadowRight = new(bar, "barHorizontalShadowRight");

        public readonly static Registry barVerticalBottom = new(bar, "barVerticalBottom");
        public readonly static Registry barVerticalMid = new(bar, "barVerticalMid");
        public readonly static Registry barVerticalTop = new(bar, "barVerticalTop");
        public readonly static Registry barVerticalShadowBottom = new(bar, "barVerticalShadowBottom");
        public readonly static Registry barVerticalShadowMid = new(bar, "barVerticalShadowMid");
        public readonly static Registry barVerticalShadowTop = new(bar, "barVerticalShadowTop");
        public readonly static Registry shield = new(bar, "shield");
        public readonly static Registry ship = new(bar, "ship");
        public readonly static Registry propulsion = new(bar, "propulsion");

    }
}
