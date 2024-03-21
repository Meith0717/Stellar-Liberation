// ScienceStation.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceStations
{
    public class ScienceStation : GameObject2D, ICollidable
    {
        public float Mass => float.PositiveInfinity;

        public ScienceStation(Vector2 position)
            : base(position, GameSpriteRegistries.scienceStation, 1, 20) { }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer scene)
        {
            base.Update(gameTime, inputState, scene);
            GameObject2DInteractionManager.Manage(inputState, this, scene, null, null, null);
        }

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
        }
    }
}
