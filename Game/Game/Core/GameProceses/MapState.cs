// MapState.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.Extensions;
using StellarLiberation.Game.Core.GameProceses.PositionManagement;
using StellarLiberation.Game.Core.Visuals;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.GameObjects.AstronomicalObjects.Types;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;
using System.Collections.Generic;
using System.IO;

namespace StellarLiberation.Game.Core.GameProceses
{
    [Serializable]
    public class MapState
    {
        [Serializable]
        private class HyperRoute(Flagship flagship, PlanetsystemState start, PlanetsystemState end)
        {
            [JsonProperty] public readonly Flagship Flagship = flagship;
            [JsonProperty] public readonly PlanetsystemState Start = start;
            [JsonProperty] public readonly PlanetsystemState End = end;
            [JsonProperty] public Vector2 Position = start.MapPosition;

            public bool Update(GameTime gameTime)
            {
                var velocity = Flagship.HyperDrive.MaxVelocity * 0.1 * gameTime.ElapsedGameTime.TotalMilliseconds;
                var startPosition = Start.MapPosition;
                var endPosition = End.MapPosition;
                var direction = startPosition.DirectionToVector2(endPosition);
                Position += direction * (float)velocity;
                if (Vector2.Distance(Position, endPosition) <= velocity * 1.2)
                {
                    flagship.IsDisposed = false;
                    End.AddGameObject(flagship);
                    HyperDriveEffect.Stop(flagship.Position, End.ParticleEmitors, 1);
                    return true;
                }
                return false;
            }

            public void Draw(float cameraZoom)
            {
                var startPosition = Start.MapPosition;
                var endPosition = End.MapPosition;
                ArrowPath.Draw(startPosition, endPosition, .5f);
                TextureManager.Instance.Draw(GameSpriteRegistries.radar, Position - new Vector2(15), 30, 30, Flagship.Fraction is Fractions.Enemys ? Color.Red : Color.LightGreen);
            }
        }

        [JsonIgnore] public readonly List<Planetsystem> Planetsystems = new();
        [JsonIgnore] public readonly SpatialHashing SpatialHasing = new(500);
        [JsonProperty] private readonly List<HyperRoute> mHyperRoutes = new();

        public void Initialize(List<PlanetsystemState> planetsystemStates)
        {
            foreach (var state in planetsystemStates)
            {
                var planetsystem = new Planetsystem(state);
                Planetsystems.Add(planetsystem);
                SpatialHasing.InsertObject(planetsystem, (int)planetsystem.Position.X, (int)planetsystem.Position.Y);
            }
        }

        public void JumpInHyperSpace(Flagship flagship, PlanetsystemState start, PlanetsystemState end) => mHyperRoutes.Add(new(flagship, start, end));

        public void Update(GameTime gameTime)
        {
            foreach (var planetSystem in Planetsystems)
                planetSystem.Update(gameTime, null, null);
            var copyHyperRoutes = new List<HyperRoute>(mHyperRoutes);
            foreach (var hyperRoute in copyHyperRoutes)
            {
                if (hyperRoute.Update(gameTime))
                    mHyperRoutes.Remove(hyperRoute);
            }
        }

        public void Draw(float cameraZoom)
        {
            foreach (var hyperRoute in mHyperRoutes)
                hyperRoute.Draw(cameraZoom);
        }
    }
}
