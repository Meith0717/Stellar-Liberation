// Player.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.AI.Behaviors.Combat;
using StellarLiberation.Game.Core.AI.Behaviors;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.ItemManagement;
using StellarLiberation.Game.Core.LayerManagement;
using StellarLiberation.Game.Core.SpaceShipManagement;
using StellarLiberation.Game.Layers;
using StellarLiberation.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;

namespace StellarLiberation.Game.GameObjects
{
    [Serializable]
    public class Player : SpaceShip
    {

        private Inventory mInventory;

        public Player() : base(Vector2.Zero, ContentRegistry.player.Name, 1, new(20000), new(1f, 0.01f), new(1000, Color.BlueViolet, 2, 2), new(100, int.MaxValue, 0, 0), Factions.Enemys)
        {
            mInventory = new(2000);

            WeaponSystem.PlaceTurret(new(new(110, 35), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(110, -35), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-130, 100), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-130, -100), 1, TextureDepth + 1));
            WeaponSystem.PlaceTurret(new(new(-150, 0), 1, TextureDepth + 1));

            mAi = new(new()
            {
                // new PatrollBehavior(),
                // new InterceptBehavior(),
                // new FarCombatBehavior(10000),
                // new FleeBehavior()
            });
        }

        public void SpawnInNewPlanetSystem(Vector2 position)
        {
            Position = position;
            Velocity = 0;
        }

        public override void Update(GameTime gameTime, InputState inputState, GameLayer gameLayer, Scene scene)
        {
            WeaponSystem.StopFire();
            inputState.DoMouseAction(MouseActionType.RightClickHold, () => WeaponSystem.Fire());
            SublightEngine.FollowMouse(inputState, scene.WorldMousePosition);
            mInventory.Update(this, scene);

            base.Update(gameTime, inputState, gameLayer, scene);
            scene.Camera.SetPosition(Position);
        }

        public override void HasCollide()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SceneManagerLayer sceneManagerLayer, Scene scene)
        {
            base.Draw(sceneManagerLayer, scene);
            mInventory.Draw(this);
        }
    }
}
