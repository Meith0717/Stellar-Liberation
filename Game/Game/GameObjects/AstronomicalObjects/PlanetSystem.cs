// PlanetSystem.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration;
using StellarLiberation.Game.Core.GameProceses.SectorManagement;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    public class PlanetSystem : GameObject2D
    {
        private bool mIsHovered;
        public int SystemRadius;
        private readonly Sector mSector;
        public readonly string Name;
        private int? PlanetCount;
        private int? Temperature;
        public readonly int Seed;
        public Fractions Occupier = Fractions.Enemys;

        public PlanetSystem(Vector2 position, int seed) : base(position, GameSpriteRegistries.star, .1f, 1)
        {
            Seed = seed;
            Name = $"SL {Seed}";

            mSector = new(position - (new Vector2(MapFactory.MapScale) / 2), MapFactory.MapScale, MapFactory.MapScale);
        }

        public List<GameObject2D> GetAstronomicalObjects()
        {
            var gameObjects = new List<GameObject2D>();
            var seededRandom = new Random(Seed);

            var star = StarGenerator.Generat(seededRandom);
            TextureColor = star.TextureColor;
            Temperature = star.Kelvin;

            gameObjects.Add(star);
            var distanceToStar = (int)star.BoundedBox.Radius;
            PlanetCount = (int)Triangular.Sample(seededRandom, 1, 10, 7);
            for (int i = 1; i <= PlanetCount; i++)
            {
                distanceToStar += seededRandom.Next(40000, 80000);
                gameObjects.Add(PlanetGenerator.GetPlanet(seededRandom, star.Kelvin, distanceToStar));
            }

            distanceToStar += 50000;
            SystemRadius = distanceToStar;

            gameObjects.AddRange(AsteroidGenerator.GetAsteroidsRing(Position, distanceToStar));
            return gameObjects;
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
