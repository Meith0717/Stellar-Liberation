using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.GameEngine.Content_Management;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Layers;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Newtonsoft.Json;
using System;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    public enum Danger { None, Moderate, Medium, High }

    [Serializable]
    public class PlanetSystem : InteractiveObject
    {
        [JsonProperty] public Star Star { get; private set; }
        [JsonProperty] public List<Planet> Planets { get; private set; }
        [JsonProperty] public List<Enemy> Pirates { get; private set; }
        [JsonProperty] public bool HasPlayer { get; private set; }
        [JsonProperty] public Danger Danger { get; private set; }
        [JsonProperty] public CircleF SystemBounding { get; private set; }

        public PlanetSystem(Vector2 mapPosition, Vector2 sectorPosition, int boundaryRadius, string textureId, Color color, Danger danger)
            : base(mapPosition, textureId, 0.01f, 1)
        {
            TextureColor = color;
            Danger = danger;
            SystemBounding = new(sectorPosition, boundaryRadius);
        }

        public void SetObjects(Star star, List<Planet> planets, List<Enemy> pirates)
        {
            Planets = planets;
            Star = star;
            Pirates = pirates;
        }

        public bool CheckIfHasPlayer(Player player)
        { 
            HasPlayer = SystemBounding.Contains(player.Position);
            return HasPlayer;
        }

        [JsonIgnore] private Color mCrosshairColor;

        public void UpdateOnMap(GameTime gameTime, InputState inputState, SceneLayer sceneLayer)
        {
            base.Update(gameTime, inputState, sceneLayer);
            mCrosshairColor = IsHover ? Color.MonoGameOrange : (HasPlayer ? Color.Green : GetColor());
        }

        private Color GetColor() => Danger switch
        {
            Danger.None => Color.White,
            Danger.Moderate => Color.Yellow,
            Danger.Medium => Color.Orange,
            Danger.High => Color.Red,
            _ => throw new NotImplementedException()
        };

        public void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer)
        {
            if (!gameLayer.FrustumCuller.CircleOnWorldView(SystemBounding)) return;
            Star.Update(gameTime, inputState, gameLayer);
            foreach (var item in Planets)
            {
                item.Update(gameTime, inputState, gameLayer);
            }
            var deleteList = new List<Enemy>();
            foreach (var item in Pirates)
            {
                if (item.DefenseSystem.HullLevel <= 0) deleteList.Add(item);
                item.Update(gameTime, inputState, gameLayer);
            }
            foreach (var item in deleteList) 
            { 
                Pirates.Remove(item);
                item.RemoveFromSpatialHashing(gameLayer);
            }
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
