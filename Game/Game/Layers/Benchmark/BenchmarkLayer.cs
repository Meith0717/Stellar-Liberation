// BenchmarkSetupLayer.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using System.Linq;

namespace StellarLiberation.Game.Layers.Benchmark
{
    internal class BenchmarkLayer : GameLayer
    {
        private readonly GameObject2DManager mGameObject2DManager;

        public BenchmarkLayer()
            : base(null, 50000)
        {
            var planetSystem = new PlanetSystem(Vector2.Zero, 1);
            var objs = planetSystem.GameObjects.ToList();
            objs.AddRange(planetSystem.GetAstronomicalObjects());
            mGameObject2DManager = new(objs, this, SpatialHashing);
        }

        public override void Destroy()
        {
            throw new System.NotImplementedException();
        }

        public override void DrawOnScreenView(SpriteBatch spriteBatch)
        {
            throw new System.NotImplementedException();
        }

        public override void DrawOnWorldView(SpriteBatch spriteBatch)
        {
            throw new System.NotImplementedException();
        }
    }
}
