using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects.Types;
using CelestialOdyssey.Game.Layers;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    public class SolarSystem : InteractiveObject
    {
        [JsonProperty] public Star Star { get; private set; }
        [JsonProperty] public List<Planet> Planets { get; private set; } = new();
        [JsonProperty] private List<SolarSystem> Neighbors = new();

        public SolarSystem(Vector2 position) 
            : base(position, ContentRegistry.pixle, 1, 1, Color.White)
        {
            Star = StarTypes.GenerateRandomStar(Vector2.Zero);
            TextureId = Star.TextureId;
            TextureScale = Star.TextureScale * 0.001f;
            UpdateBoundBox();
        }

        public override void Draw()
        {
            base.Draw();
            TextureManager.Instance.DrawGameObject(this, IsHover);
            TextureManager.Instance.Draw(ContentRegistry.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, TextureDepth - 1, Star.StarColor);
            TextureManager.Instance.Draw(ContentRegistry.targetCrosshair, Position, TextureScale * 2f, Rotation, TextureDepth - 1, IsHover ? Color.Blue : Color.MonoGameOrange);
            foreach (var neighbor in Neighbors)
            {
                TextureManager.Instance.DrawAdaptiveLine(Position, neighbor.Position, new(10, 10, 10, 10), 1, 1, GameLayer.Camera.Zoom);
            }
        }

        public override void Update(GameTime gameTime, InputState inputState)
        {
            base.Update(gameTime, inputState);
        }

        public override void OnPressAction()
        {
            System.Diagnostics.Debug.WriteLine("SYSTEM PRESSED");
            GameLayer.Camera.MoveToTarget(Position);
            if (!BoundedBox.Contains(GameLayer.Camera.Position)) return;
            GenerateSystemIF(Planets.Count == 0);
            GameLayer.LayerManager.AddLayer(new SystemLayer(this));
        }

        private void GenerateSystemIF(bool b)
        {
            if (!b) return;
            System.Diagnostics.Debug.WriteLine("GENERATE SYSTEM");
            Planets = PlanetGenerator.Generate(Star);
        }

        public void GetnNighbors(int amount)
        {
            List<SolarSystem> solarSystems = GameLayer.GetSortedObjectsInRadius<SolarSystem>(Position, 1000);
            if (solarSystems.Count == 0) throw new System.Exception();
            amount = (solarSystems.Count >= amount) ? amount : solarSystems.Count;
            for (int i = 1; i <= amount; i++)
            {
                var system = solarSystems[i];
                if (Neighbors.Contains(system)) continue;
                Neighbors.Add(system);
                system.Neighbors.Add(this);
            }
        }
    }
}
