// ResolutionManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.CoreProceses.ResolutionManagement
{
    public class ResolutionManager
    {
        public Resolution ActualResolution { get; private set; }
        private readonly GraphicsDeviceManager mGraphicsManager;
        public readonly List<Resolution> mResolutions = new()
        {
            new(1280, 720, .5f),
            new(1920, 1080, 1),
        };

        public ResolutionManager(GraphicsDeviceManager graphicsManager) 
        {
            mGraphicsManager = graphicsManager;
        }

        public void GetNativeResolution()
        {
            var width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            var height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }

        public bool WasResized
        {
            get
            {
                var tmp = mWasResized;
                mWasResized = false;
                return tmp;
            }
        }
        private bool mWasResized;

        public void Apply(Resolution resolution)
        {
            mGraphicsManager.PreferredBackBufferWidth = resolution.Width;
            mGraphicsManager.PreferredBackBufferHeight = resolution.Height;
            mGraphicsManager.ApplyChanges();
            ActualResolution = resolution;
            mWasResized = true;
        }

        public void ToggleFullscreen()
        {
            mGraphicsManager.ToggleFullScreen();
            mGraphicsManager.ApplyChanges();
            mWasResized = true;
        }
    }
}
