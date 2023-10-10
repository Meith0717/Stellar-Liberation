﻿using Microsoft.Xna.Framework.Content;

namespace CelestialOdyssey.Game.Core.ConfigReader
{
    public sealed class Configs
    {
        private static Configs mInstance;
        public static void Load(ContentManager content) { mInstance ??= new(content); }

        private Configs(ContentManager content)
        {
        }

    }
}
