using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.Core.MapSystem;
using CelestialOdyssey.Game.GameObjects.AstronomicalObjects.Types;
using CelestialOdyssey.Game.GameObjects.SpaceShips;
using CelestialOdyssey.Game.Layers;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CelestialOdyssey.Game.GameObjects.AstronomicalObjects
{
    [Serializable]
    public class SolarSystem : InteractiveObject
    {
        [JsonProperty] 
        private Star mStar;

        [JsonProperty]
        private SystemLayer mSystemLayer;

        [JsonIgnore]
        public bool HasPlayer;

        [JsonIgnore]
        public bool IsPath;

        public SolarSystem(Vector2 position) 
            : base(position, ContentRegistry.pixle, 0.025f, 1, Color.White)
        {
            mStar = StarTypes.GenerateRandomStar(Vector2.Zero);
            TextureId = mStar.TextureId;
            UpdateBoundBox();
        }

        public override void Draw()
        {
            base.Draw();
            TextureManager.Instance.DrawGameObject(this, IsHover);
            TextureManager.Instance.Draw(ContentRegistry.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, TextureDepth - 1, mStar.StarColor);
            var color = IsHover ? Color.MonoGameOrange : (HasPlayer ? Color.Blue : IsPath ? Color.Red : (mSystemLayer is null ? Color.Gray : Color.Green));
            TextureManager.Instance.Draw(ContentRegistry.targetCrosshair, Position, TextureScale * 1.5f, Rotation, TextureDepth - 1, color);
        }

        public override void Update(GameTime gameTime, InputState inputState) 
        { 
            base.Update(gameTime, inputState);
            HasPlayer = GetPlayer(out var _); 
        }

        public override void OnRightPressAction()
        {
            GameLayer.Camera.MoveToTarget(Position);
            if (mSystemLayer is null) return;
            if (!BoundedBox.Contains(GameLayer.Camera.Position)) return;
            GameLayer.LayerManager.AddLayer(GetLayer());
        }

        public override void OnLeftPressAction() { }

        public void AddGameObject(GameObject gameObject)
        {
            if (mSystemLayer is null) GetLayer();
            mSystemLayer.AddObject(gameObject);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            mSystemLayer.RemoveObject(gameObject);
        }

        public SystemLayer GetLayer()
        {
            if (mSystemLayer is not null) return mSystemLayer;
            mSystemLayer = new SystemLayer();
            mSystemLayer.AddObject(mStar);
            foreach (var planet in PlanetGenerator.Generate(mStar))
            {
                mSystemLayer.AddObject(planet);
            }
            return mSystemLayer;
        }


        public bool GetPlayer(out Player player)
        {
            player = null;
            if (mSystemLayer is null) return false;
            IEnumerable<Player> objs = mSystemLayer.GameObjects.OfType<Player>();
            if (!objs.Any()) return false;
            player = objs.First();
            return true;
        }
    }
}
