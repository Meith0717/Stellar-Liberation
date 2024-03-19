// GameLayerManager.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.Debugging;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.CoreProceses.Persistance;
using StellarLiberation.Game.Core.CoreProceses.ResolutionManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using StellarLiberation.Game.Layers.GameLayers;
using StellarLiberation.Game.Layers.MenueLayers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarLiberation.Game.Layers
{
    [Serializable]
    public class GameLayerManager : Layer
    {
        [JsonIgnore] public List<PlanetSystem> mPlanetSystems { get; private set; }
        [JsonIgnore] public readonly DebugSystem DebugSystem = new();
        [JsonIgnore] private readonly LinkedList<Layer> mLayers = new();
        [JsonIgnore] private readonly FrameCounter mFrameCounter = new(1000);
        [JsonProperty] public readonly SpaceshipTracer SpaceShips = new();
        [JsonProperty] private readonly MapConfig mMapConfig;
        [JsonProperty] public readonly Wallet Wallet = new();

        public GameLayerManager(Game1 game1) : base(game1, false)
        {
            mMapConfig = new(50, 50, 42);
            mPlanetSystems = MapFactory.Generate(mMapConfig);
            for (int i = 0; i < 2; i++)
                SpaceShips.AddSpaceShip(mPlanetSystems.First(), SpaceshipFactory.Get(ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, mPlanetSystems.First().SystemRadius)), ShipID.Destroyer, Fractions.Allied));
            AddLayer(new PlanetSystemLayer(this, mPlanetSystems.First(), game1));

        }

        public void AddLayer(Layer layer)
        {
            mLayers.AddLast(layer);
        }

        public void PopLayer()
        {
            if (mLayers.Count == 0) return;
            mLayers.Last.Value.Destroy();
            mLayers.RemoveLast();
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            mFrameCounter.Update(gameTime);
            DebugSystem.Update(inputState);
            inputState.DoAction(ActionType.ESC, () => LayerManager.AddLayer(new PauseLayer(this, Game1)));
            mLayers.Last?.Value.Update(gameTime, inputState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mFrameCounter.UpdateFrameCouning();
             GraphicsDevice.Clear(Color.Black);
            mLayers.Last?.Value.Draw(spriteBatch);
            spriteBatch.Begin();
            DebugSystem.ShowInfo(new(10, 10));
            TextureManager.Instance.DrawString(FontRegistries.debugFont, new Vector2(1, 1), $"{MathF.Round(mFrameCounter.CurrentFramesPerSecond)} fps", 0.75f, Color.White);
            spriteBatch.End();
        }

        public override void Destroy() { }

        public override void ApplyResolution() { }
    }
}
