// SpaceCraft.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses;
using StellarLiberation.Game.Core.GameProceses.SpaceShipProceses.Weapons;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.Layers;
using System;
using System.Collections.Generic;

namespace StellarLiberation.Game.GameObjects.Spacecrafts
{
    [Serializable]
    public abstract class Spacecraft : GameObject, IGameObject, ICollidable
    {
        [JsonProperty] public float Mass => MathF.PI * 1.33f * BoundedBox.Radius;
        [JsonProperty] public Defense Defense { get; private set; }
        [JsonProperty] public WeaponManager Weapons { get; private set; }
        [JsonIgnore] public readonly Sensors Sensors;
        [JsonProperty] public readonly Fractions Fraction;
        [JsonProperty] private readonly Color mAccentColor;
        [JsonProperty] private readonly Vector2 mEngineTrailPosition;

        public Spacecraft(Vector2 position, Fractions fraction, string textureID, float textureScale, Vector2 engineTrailPosition)
            : base(position, textureID, textureScale, 10)
        {
            Sensors = new();
            Fraction = fraction;
            mAccentColor = Fraction switch
            {
                Fractions.Allied => Color.LightBlue,
                Fractions.Enemys => Color.MonoGameOrange,
                Fractions.Neutral => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
            mEngineTrailPosition = engineTrailPosition;
        }

        protected void Populate(float shieldForce, float hullForce, float shieldReg, float hullReg, List<Weapon> weapons)
        {
            Defense = new(shieldForce, hullForce, shieldReg, hullReg);
            Weapons = new(weapons);
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            if (Defense is null) throw new ArgumentNullException("ApplyConfig not called!");
            Defense.Update(this, gameTime, gameState.GameSettings, planetsystemState);
            Weapons.Update(gameTime, this, planetsystemState);
            Sensors.Scan(gameTime, this, Fraction, planetsystemState);

            MovingDirection = Geometry.CalculateDirectionVector(Rotation);
            Physics.HandleCollision(gameTime, this, planetsystemState.SpatialHashing);
            GameObjectMover.Move(gameTime, this, planetsystemState.SpatialHashing);
            TrailEffect.Show(Transformations.Rotation(Position, mEngineTrailPosition, Rotation), MovingDirection, Velocity, gameTime, mAccentColor, planetsystemState.ParticleEmitors, gameState.GameSettings.ParticlesMultiplier, 2);

            if (Defense.HullPercentage <= 0) Explode(gameState, planetsystemState);
            base.Update(gameTime, gameState, planetsystemState);
        }

        private void Explode(GameState gameState, PlanetsystemState gameLayer)
        {
            ExplosionEffect.ShipDestroyed(Position, gameLayer.ParticleEmitors, gameState.GameSettings.ParticlesMultiplier);
            IsDisposed = true;
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);

            TextureManager.Instance.Draw($"{TextureId}Borders", Position, TextureScale, Rotation, TextureDepth, mAccentColor);
            TextureManager.Instance.Draw($"{TextureId}Frame", Position, TextureScale, Rotation, TextureDepth, Color.White);
            TextureManager.Instance.Draw($"{TextureId}Hull", Position, TextureScale, Rotation, TextureDepth, new(30, 30, 35));
            TextureManager.Instance.Draw($"{TextureId}Structure", Position, TextureScale, Rotation, TextureDepth, new(20, 20, 50));
            Weapons.Draw(TextureScale, new(30, 30, 35));

            Defense.Draw(this);
        }

        public void HasCollide(Vector2 position, GameLayer scene) {; }
    }
}
