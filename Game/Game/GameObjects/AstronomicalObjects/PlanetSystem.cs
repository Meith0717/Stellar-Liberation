// PlanetSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Distributions;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.MapGeneration;
using StellarLiberation.Game.Core.GameProceses.MapGeneration.ObjectsGeneration;
using StellarLiberation.Game.Core.GameProceses.SectorManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips;
using StellarLiberation.Game.Layers;
using System;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    [Serializable]
    public class PlanetSystem : GameObject2D
    {
        [JsonIgnore] public GameObjectManager AstronomicalObjsManager { get; private set; }
        [JsonIgnore] private PlanetSystemInstance mInstance;
        [JsonIgnore] private bool mIsHovered;

        [JsonIgnore] private readonly Sector mSector;
        [JsonIgnore] private readonly string Name;
        [JsonProperty] private int? PlanetCount;
        [JsonProperty] private int? Temperature;
        [JsonProperty] public readonly GameObjectManager GameObjectManager;
        [JsonProperty] private readonly int mSeed;
        [JsonProperty] public Fractions Occupier = Fractions.Enemys;

        public PlanetSystem(Vector2 position, int seed) : base(position, GameSpriteRegistries.star, .1f, 1)
        {
            mSeed = seed;
            GameObjectManager = new();

            Name = $"SL-";
            var rand = new Random(mSeed);
            for (var i = 0; i < 10; i++) Name += rand.Next(0, 9);

            mSector = new(position - (new Vector2(MapFactory.MapScale) / 2), MapFactory.MapScale, MapFactory.MapScale);

            for (int i = 0; i < 5; i++) GameObjectManager.AddObj(SpaceShipFactory.Get(ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 100000)), ShipID.Bomber, Fractions.Enemys));
            //for (int i = 0; i < 5; i++) GameObjectManager.AddObj(SpaceShipFactory.Get(ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 100000)), ShipID.Cargo, Fractions.Enemys));
            //for (int i = 0; i < 5; i++) GameObjectManager.AddObj(SpaceShipFactory.Get(ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 100000)), ShipID.Cuiser, Fractions.Enemys));
            //for (int i = 0; i < 5; i++) GameObjectManager.AddObj(SpaceShipFactory.Get(ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 100000)), ShipID.Destroyer, Fractions.Enemys));
            //for (int i = 0; i < 5; i++) GameObjectManager.AddObj(SpaceShipFactory.Get(ExtendetRandom.NextVectorInCircle(new(Vector2.Zero, 100000)), ShipID.Corvette, Fractions.Enemys));
        }

        public PlanetSystemInstance GetInstance()
        {
            Occupier = Fractions.Allied;
            if (mInstance is not null) return mInstance;

            var seededRandom = new Random(mSeed);
            AstronomicalObjsManager = new GameObjectManager();

            var star = StarGenerator.Generat(seededRandom);
            TextureColor = star.TextureColor;
            Temperature = star.Kelvin;

            AstronomicalObjsManager.AddObj(star);
            var distanceToStar = (int)star.BoundedBox.Radius;
            PlanetCount = (int)Triangular.Sample(seededRandom, 1, 10, 7);
            for (int i = 1; i <= PlanetCount; i++)
            {
                distanceToStar += seededRandom.Next(40000, 80000);
                AstronomicalObjsManager.AddObj(PlanetGenerator.GetPlanet(seededRandom, star.Kelvin, distanceToStar));
            }

            distanceToStar += 50000;

            AstronomicalObjsManager.AddRange(AsteroidGenerator.GetAsteroidsRing(Position, distanceToStar));
            mInstance = new(GameObjectManager, AstronomicalObjsManager, star);
            return mInstance;
        }

        public void ClearInstance() => mInstance = null;

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {

            void LeftPressAction()
            {
                // scene.GameLayer.LayerManager.AddLayer(new EventPopup($"Do you want to travel to\n{Name}?", () =>
                // {
                //     scene.GameLayer.HudLayer.Hide = false;
                //     scene.GameLayer.PopScene();
                //     scene.GameLayer.Player.HyperDrive.SetTarget(this);
                // }, null));
            };

            mSector.Update(Occupier);

            base.Update(gameTime, inputState, scene);
            mIsHovered = false;
            GameObject2DInteractionManager.Manage(inputState, this, scene, LeftPressAction, null, () => mIsHovered = true); ;
        }

        public override void Draw(Scene scene)
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
