// Path.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.



using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.Utilitys;
using System;

namespace StellarLiberation.Game.Core.Visuals
{
    public static class ArrowPath
    {
        private static readonly int TextureSize = 32;

        public static void Draw(Vector2 start, Vector2 end, float scale)
        {
            var textureSize = TextureSize * scale;
            var dir = start.DirectionToVector2(end);
            var length = Vector2.Distance(start, end);
            var steps = (int)Math.Round(length / textureSize, 0);
            var rotarion = Geometry.AngleBetweenVectors(start, end);
            var position = end;
            for (int _ = 0; _ < steps; _++)
            {
                TextureManager.Instance.Draw(GameSpriteRegistries.arrow, position, scale, rotarion, 1, Color.MonoGameOrange * .5f);
                position -= dir * textureSize;
            }
        }
    }
}
