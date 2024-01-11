// MapPlanetSystem.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using MonoGame.Extended;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;

namespace StellarLiberation.Game.GameObjects.AstronomicalObjects.Types
{
    public class MapPlanetSystem : GameObject2D
    {
        public readonly PlanetSystem mPlanetSystem;

        public MapPlanetSystem(Vector2 position, PlanetSystem planetSystem, string textureId, Color textureColor) : base(position, textureId, 0.01f, 1)
        {
            mPlanetSystem = planetSystem;
            TextureColor = textureColor;
        }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            void LeftPressAction()
            {
                scene.GameLayer.HudLayer.Hide = false;
                scene.GameLayer.PopScene();
                scene.GameLayer.Player.HyperDrive.SetTarget(mPlanetSystem);
            };

            base.Update(gameTime, inputState, scene);
            GameObject2DInteractionManager.Manage(inputState, this, scene, LeftPressAction, null, null);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw(GameSpriteRegistries.starLightAlpha, Position, TextureOffset, TextureScale * 2f, Rotation, 0, TextureColor);
        }

    }
}
