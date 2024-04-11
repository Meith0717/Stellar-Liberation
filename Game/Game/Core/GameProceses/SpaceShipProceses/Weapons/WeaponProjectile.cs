// WeaponProjectile.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using StellarLiberation.Game.Layers;
using System;

namespace StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons
{
    [Serializable]
    public class WeaponProjectile : GameObject, IGameObject
    {
        [JsonProperty] public float ShieldDamage { get; private set; }
        [JsonProperty] public float HullDamage { get; private set; }
        [JsonProperty] public Spacecraft Shooter { get; private set; }
        [JsonProperty] private Spacecraft Target;
        [JsonProperty] private bool mFollowTarget;

        public WeaponProjectile(Vector2 startPosition, string textureId)
            : base(startPosition, textureId, 1f, 15) {; }

        public void Populate(Spacecraft shooter, Spacecraft target, float shootRotation, float shieldDamage, float hullDamage, float range, bool followTarget, Color color)
        {
            Shooter = shooter;
            Target = target;
            Rotation = shootRotation;
            ShieldDamage = shieldDamage;
            HullDamage = hullDamage;
            mFollowTarget = followTarget;
            TextureColor = color;
            Velocity = 10;
            DisposeTime = range / Velocity;
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            if (mFollowTarget && Target is not null)
                Rotation = Geometry.AngleBetweenVectors(Position, Target.Position);
            MovingDirection = Geometry.CalculateDirectionVector(Rotation);
            GameObjectMover.Move(gameTime, this, planetsystemState.SpatialHashing);
            base.Update(gameTime, gameState, planetsystemState);
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);
            TextureManager.Instance.DrawGameObject(this);
        }

        public void HasCollide(Vector2 position, GameLayer scene)
        {
            IsDisposed = true;
            Position = position;
            Velocity = 0;
        }
    }
}