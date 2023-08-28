using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects.Types;
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

        public SolarSystem(Vector2 position) 
            : base(position, ContentRegistry.pixle, 1, 1, Color.Gray)
        {
            Star = StarTypes.GenerateRandomStar(Position);
            TextureId = Star.TextureId;
            TextureScale = 50;
            UpdateBoundBox();
        }

        public override void Draw()
        {
            base.Draw();
            TextureManager.Instance.DrawGameObject(this, IsHover);
        }

        public override void OnPressAction()
        {
            System.Diagnostics.Debug.WriteLine("PRESSED");
        }
    }
}
