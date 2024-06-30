// EffectManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarLiberation.Game.Core.CoreProceses.ContentManagement
{
    public sealed class EffectManager
    {
        private static EffectManager mInstance;
        public static EffectManager Instance
            => mInstance is null ? mInstance = new() : mInstance;
        public  Effect Effect { get; private set; }

        public void LoadBuildContent(ContentManager content, string path) => Effect = content.Load<Effect>(path);
    }
}
