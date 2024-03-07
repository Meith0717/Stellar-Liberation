// Container.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.GameObjects.Recources.Items;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.Recources
{
    public class Container : GameObject2D
    {
        public readonly List<Item> Items;

        public Container(Vector2 position, List<Item> items) 
            : base(position, GameSpriteRegistries.container, .5f, 20) 
            => Items = items;

        public override void Draw(GameLayer scene)
        {
            base.Draw(scene);
            TextureManager.Instance.DrawGameObject(this);
            TextureManager.Instance.Draw(GameSpriteRegistries.radar, Position, .04f / scene.Camera2D.Zoom, 0, TextureDepth + 1, Color.LightGray);

        }
    }
}
