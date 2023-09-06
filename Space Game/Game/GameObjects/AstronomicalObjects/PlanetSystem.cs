using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.Utility;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    public class PlanetSystem : InteractiveObject
    {
        [JsonProperty] public Star Star { get; private set; }
        [JsonProperty] public List<Planet> Planets { get; private set; }
        [JsonProperty] private CircleF mSectorBounding;
        [JsonIgnore] public bool HasPlayer { get; private set; }
        [JsonIgnore] private Color mCrosshairColor;

        public PlanetSystem(Vector2 mapPosition, Vector2 sectorPosition, int boundaryRadius, string textureId, Color color) 
            : base(mapPosition, textureId, 0.01f, 1)
        {
            TextureColor = color;
            mSectorBounding = new(sectorPosition, boundaryRadius);
        }

        public void SetAstronomicalObjects(Star star, List<Planet> planets)
        {
            Planets = planets;
            Star = star;
        }

        public bool CheckIfHasPlayer(Player player) 
        { 
            HasPlayer = mSectorBounding.Contains(player.Position);
            return HasPlayer;
        }

        public Vector2 GetEntryPosition(Player player)
        {
            return Geometry.GetPointOnCircle(mSectorBounding, Geometry.AngleBetweenVectors(mSectorBounding.Position, player.Position));
        }

        public override void Update(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            base.Update(gameTime, inputState, sceneLayer);
            mCrosshairColor = IsHover ? Color.MonoGameOrange : (HasPlayer ? Color.Green : Color.White);
        }

        public override void Draw(SceneLayer sceneLayer)
        {
            base.Draw(sceneLayer);
            TextureManager.Instance.DrawGameObject(this, IsHover);
            TextureManager.Instance.Draw(ContentRegistry.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, 0, TextureColor);
            TextureManager.Instance.Draw(ContentRegistry.mapCrosshair, Position, TextureScale * 2, Rotation, 1, mCrosshairColor);
        }

        public override void LeftPressAction() { ; }

        public override void RightPressAction() { ; }
    }
}
