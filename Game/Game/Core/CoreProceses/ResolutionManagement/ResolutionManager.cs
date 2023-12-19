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
                new Resolution(1024, 768, 4.25f),       // 4:3      XGA
                new Resolution(1280, 720, 0.8333f),     // 16:9     HD
                new Resolution(1280, 800, 1.25f),       // 8:5      WXGA
                new Resolution(1366, 768, 1.1842f),     // 16:9     HD
                new Resolution(1440, 900, 1.1667f),     // 16:10    WXGA+
                new Resolution(1600, 900, 1.0625f),     // 16:9     HD+
                new Resolution(1600, 1200, 1.0f),       // 4:3      UXGA
                new Resolution(1680, 1050, 0.9706f),    // 16:10    WSXGA+
                new Resolution(1920, 1080, 1.0f),       // 16:9     Full HD
                new Resolution(1920, 1200, 1.0f),       // 16:10    WUXGA
                new Resolution(2560, 1080, 0.875f),     // 64:27    UltraWide
                new Resolution(2560, 1440, 0.8478f),    // 16:9     Quad HD
                new Resolution(2560, 1600, 0.8125f),    // 16:10    WQXGA
                new Resolution(3440, 1440, 0.8478f),    // 21:9     UltraWide QHD
                new Resolution(3840, 2160, 1.5f)        // 16:9     4K UHD
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
