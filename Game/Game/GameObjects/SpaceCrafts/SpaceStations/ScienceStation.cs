// ScienceStation.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameObjectManagement;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceStations
{
    public class ScienceStation : GameObject2D
    {
        public ScienceStation(Vector2 position) 
            : base(position, GameSpriteRegistries.scienceStation, 1, 20) { }

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            RemoveFromSpatialHashing(scene);
            base.Update(gameTime, inputState, scene);
            AddToSpatialHashing(scene);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
