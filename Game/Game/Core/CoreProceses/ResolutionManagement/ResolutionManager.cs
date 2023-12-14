// ResolutionManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Core.CoreProceses.ResolutionManagement
{
    public class ResolutionManager
    {
        public float UiScaling = 1;
        private readonly GraphicsDeviceManager mGraphicsManager;
        public readonly Dictionary<string, Resolution> mResolutions = new();

        public ResolutionManager(GraphicsDeviceManager graphicsManager)
        {
            mGraphicsManager = graphicsManager;
            var lst = new List<Resolution>()
            { 
                new(800,480, .5f),      // default
                new(1920, 1280, 1),     // 3:2      Full HD
                new(1920, 1200, 1),     // 16:10    Full HD
                new(1920, 1080, 1),     // 16:9     Full HD
                new(2560, 1080, 1),     // 21:9     Full HD

                new(2560, 1440, 1.333f) // 16:9     QHD 
            };
            foreach (var resolution in lst) mResolutions[resolution.ToString()] = resolution;
        }

        public void GetNativeResolution()
        {
            var width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            var height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            var resolution = $"{width}x{height}";
            if (Apply(resolution)) return;
            throw new System.Exception("Resolution not Found");
        }

        public List<string> Resolutions => mResolutions.Keys.ToList();

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

        public bool Apply(string resolutionId)
        {
            try
            {
                var resolution = mResolutions[resolutionId];
                mGraphicsManager.PreferredBackBufferWidth = resolution.Width;
                mGraphicsManager.PreferredBackBufferHeight = resolution.Height;
                mGraphicsManager.ApplyChanges();
                UiScaling = resolution.uiScaling;
                mWasResized = true;
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public void ToggleFullscreen()
        {
            mGraphicsManager.ToggleFullScreen();
            mWasResized = true;            
        }
    }
}
