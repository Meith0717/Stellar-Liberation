// JsonElementExtension.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using System.Text.Json;

namespace StellarLiberation.Game.Core.Extensions
{
    internal static class JsonElementExtension
    {
        public static Vector2 GetVector(this JsonElement jsonElement) => new Vector2(jsonElement.GetProperty("X").GetInt32(), jsonElement.GetProperty("Y").GetInt32());

        public static Color GetColor(this JsonElement jsonElement) => new Color(jsonElement.GetProperty("R").GetInt32(), jsonElement.GetProperty("G").GetInt32(), jsonElement.GetProperty("B").GetInt32(), jsonElement.GetProperty("A").GetInt32());

    }
}
