﻿using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.Core.Parallax;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CelestialOdyssey.Game.Layers
{
    [Serializable]
    public class MapLayer : GameLayer
    {
        [JsonIgnore] private readonly ParllaxManager mParllaxManager;
        [JsonProperty] private readonly Map map = new();
        [JsonProperty] public Player Player { get; set; } = new(Vector2.Zero);

        public MapLayer() : base(100)
        {
            map.Generate(this);

            var startSystem = map.GetRandomSystem();
            Camera.SetPosition(startSystem.Position);
            startSystem.AddGameObject(Player);

            mParllaxManager = new();
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax.Name, 0.1f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax1.Name, 0.15f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax2.Name, 0.2f));
            mParllaxManager.Add(new(ContentRegistry.gameBackgroundParlax3.Name, 0.25f));
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
            mParllaxManager.Update(Camera.Movement, Camera.Zoom);
            if (inputState.mMouseActionType == MouseActionType.RightClick)
            {
                foreach (var starget in GameObjects.OfType<SolarSystem>())
                {
                    if (!starget.IsHover) continue;
                    map.GetActualSystem(out var system);
                    map.GetPath(system, starget);
                }
            }
            if (inputState.mActionList.Contains(ActionType.Map))
            {
                if (map.GetActualSystem(out var system))
                {
                    Camera.SetPosition(system.Position);
                    LayerManager.AddLayer(system.GetLayer());
                } 
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mParllaxManager.Draw();
            spriteBatch.End();
            base.Draw(spriteBatch);
        }
        public override void OnResolutionChanged() { mParllaxManager.OnResolutionChanged(GraphicsDevice); }

        public override void Destroy() { }

        public override void DrawOnWorld() { map.DrawEdges(); }

    }
}
