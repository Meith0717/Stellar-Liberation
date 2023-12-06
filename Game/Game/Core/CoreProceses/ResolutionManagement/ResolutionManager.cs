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
        private readonly LayerManager mLayerManager;
        public readonly List<Resolution> mResolutions = new()
        {
            new(1280, 720, .5f),
            new(1920, 1080, 1),
        };

        public ResolutionManager(GraphicsDeviceManager graphicsManager, LayerManager layerManager) 
        {
            mGraphicsManager = graphicsManager;
            mLayerManager = layerManager;
        }

        public void GetNativeResolution()
        {
            var width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            var height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }

        public void Apply(Resolution resolution)
        {
            mGraphicsManager.PreferredBackBufferWidth = resolution.Width;
            mGraphicsManager.PreferredBackBufferHeight = resolution.Height;
            mLayerManager.OnResolutionChanged();
            ActualResolution = resolution;
        }

        public void ToggleFullscreen()
        {
            mGraphicsManager.ToggleFullScreen();
            mLayerManager.OnResolutionChanged();
        }
    }
}
