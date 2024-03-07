// PlanetSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration;
using StellarLiberation.Game.Core.GameProceses.SectorManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    [Serializable]
    public class PlanetSystem : GameObject2D
    {
        [JsonIgnore] public GameObject2DManager AstronomicalObjs { get; private set; }
        [JsonIgnore] private bool mIsHovered;

        [JsonIgnore] private readonly Sector mSector;
        [JsonIgnore] private readonly string Name;
        [JsonIgnore] public int SystemRadius { get; private set; }
        [JsonProperty] private int? PlanetCount;
        [JsonProperty] private int? Temperature;
        [JsonProperty] public GameObject2DManager GameObjects { get; private set; }
        [JsonProperty] private readonly int mSeed;
        [JsonProperty] public Fractions Occupier = Fractions.Enemys;

        public PlanetSystem(Vector2 position, int seed) : base(position, GameSpriteRegistries.star, .1f, 1)
        {
            mSeed = seed;
            GameObjects = new();

            Name = $"SL-";
            var rand = new Random(mSeed);
            for (var i = 0; i < 10; i++) Name += rand.Next(0, 9);

            for (int i = 0; i < 50; i++) SpaceshipFactory.Spawn(this, ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 250000)), ShipID.Bomber, Fractions.Enemys, out var _);
            for (int i = 0; i < 50; i++) SpaceshipFactory.Spawn(this, ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 250000)), ShipID.Bomber, Fractions.Allied, out var _);

            mSector = new(position - (new Vector2(MapFactory.MapScale) / 2), MapFactory.MapScale, MapFactory.MapScale);
        }

        public void GenerateAstronomicalObjects()
        {
            if (AstronomicalObjs is not null) return;
            AstronomicalObjs = new();
            Occupier = Fractions.Allied;
            var seededRandom = new Random(mSeed);

            var star = StarGenerator.Generat(seededRandom);
            TextureColor = star.TextureColor;
            Temperature = star.Kelvin;

            AstronomicalObjs.Add(star);
            var distanceToStar = (int)star.BoundedBox.Radius;
            PlanetCount = (int)Triangular.Sample(seededRandom, 1, 10, 7);
            for (int i = 1; i <= PlanetCount; i++)
            {
                distanceToStar += seededRandom.Next(40000, 80000);
                AstronomicalObjs.Add(PlanetGenerator.GetPlanet(seededRandom, star.Kelvin, distanceToStar));
            }

            distanceToStar += 50000;
            SystemRadius = distanceToStar;

            AstronomicalObjs.AddRange(AsteroidGenerator.GetAsteroidsRing(Position, distanceToStar));
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer)
        {

            void LeftPressAction()
            {
                gameLayer.GameState.PopLayer();
                gameLayer.GameState.Player.HyperDrive.SetTarget(this);
            };

            mSector.Update(Occupier);

            base.Update(gameTime, inputState, gameLayer);
            mIsHovered = false;
            GameObject2DInteractionManager.Manage(inputState, this, gameLayer, LeftPressAction, null, () => mIsHovered = true); ;
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            var color = mIsHovered ? Color.DarkGray : TextureColor;

            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw(GameSpriteRegistries.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, 0, color);

            TextureManager.Instance.DrawString(FontRegistries.titleFont, BoundedBox.ToRectangleF().BottomRight, Name, 0.1f, color);

            TextureManager.Instance.Draw(MenueSpriteRegistries.temperature, BoundedBox.ToRectangleF().BottomRight + new Vector2(0, 20), 0.2f, 0, 1, color);
            TextureManager.Instance.DrawString(FontRegistries.titleFont, BoundedBox.ToRectangleF().BottomRight + new Vector2(8, 20), Temperature is null ? "?" : $"{Temperature} K", 0.1f, color);

            TextureManager.Instance.Draw(MenueSpriteRegistries.planet, BoundedBox.ToRectangleF().BottomRight + new Vector2(0, 40), 0.2f, 0, 1, color);
            TextureManager.Instance.DrawString(FontRegistries.titleFont, BoundedBox.ToRectangleF().BottomRight + new Vector2(8, 40), PlanetCount is null ? "?" : PlanetCount.ToString(), 0.1f, color);

            mSector.Draw();
        }

    }
}
