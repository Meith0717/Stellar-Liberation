// SpaceShip.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using MathNet.Numerics.Random;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement;
using StellarLiberation.Game.Core.CoreProceses.ContentManagement.ContentRegistry;
using StellarLiberation.Game.Core.CoreProceses.LayerManagement;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.Core.GameProceses.AI;
using StellarLiberation.Game.Core.GameProceses.CollisionDetection;
using StellarLiberation.Game.Core.GameProceses.GameObjectManagement;
using StellarLiberation.Game.Core.GameProceses.RecourceManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipComponents;
using StellarLiberation.Game.Core.Utilitys;
using StellarLiberation.Game.Core.Visuals.ParticleSystem.ParticleEffects;
using StellarLiberation.Game.Layers;
using System;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.Spaceships
{
    [Serializable]
    public class Spaceship : GameObject2D, ICollidable
    {
        [JsonProperty] public float Mass { get => 5; }
        [JsonProperty] public Defense Defense { get; private set; }
        [JsonProperty] public HyperDrive HyperDrive { get; private set; }
        [JsonProperty] public ImpulseDrive ImpulseDrive { get; private set; }

        [JsonProperty] public readonly Sensors Sensors;
        [JsonProperty] public readonly Fractions Fraction;
        [JsonProperty] private readonly Color mAccentColor;


        public Spaceship(Vector2 position, Fractions fraction, string textureID, float textureScale)
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
        }

        public void ApplyConfig(float shieldForcePerc, float hullForcePerc, float shieldRegPerc, float hullRegPerc, float impulseVelocityPerc, float hyperVelocityPerc)
        {
            Defense = new(shieldForcePerc, hullForcePerc, shieldRegPerc, hullRegPerc);
            HyperDrive = new(impulseVelocityPerc);
            ImpulseDrive = new(hyperVelocityPerc);
        }

        public override void Update(GameTime gameTime, GameState gameState, PlanetsystemState planetsystemState)
        {
            ImpulseDrive.Update(gameTime, this, Defense.HullPercentage);

            MovingDirection = Geometry.CalculateDirectionVector(Rotation);
            Physics.HandleCollision(gameTime, this, planetsystemState.SpatialHashing);
            GameObject2DMover.Move(gameTime, this, planetsystemState.SpatialHashing);
            base.Update(gameTime, gameState, planetsystemState);
            TrailEffect.Show(Transformations.Rotation(Position, new(-800, 0), Rotation), MovingDirection, Velocity, gameTime, mAccentColor, planetsystemState.ParticleEmitors, gameState.GameSettings.ParticlesMultiplier);

            Sensors.Scan(gameTime, this, Fraction, planetsystemState);
            HyperDrive.Update(gameTime, this, planetsystemState);
            Defense.Update(this, gameTime, gameState.GameSettings, planetsystemState);

            if (Defense.HullPercentage <= 0) Explode(gameState, planetsystemState);
        }

        private void Explode(GameState gameState, PlanetsystemState gameLayer)
        {
            ExplosionEffect.ShipDestroyed(Position, gameLayer.ParticleEmitors, gameState.GameSettings.ParticlesMultiplier);
            IsDisposed = true;
            // TODO Cam Shaker
        }

        public override void Draw(GameState gameState, GameLayer scene)
        {
            base.Draw(gameState, scene);

            TextureManager.Instance.Draw($"{TextureId}Borders", Position, TextureScale, Rotation, TextureDepth, mAccentColor);
            TextureManager.Instance.Draw($"{TextureId}Frame", Position, TextureScale, Rotation, TextureDepth, Color.White);
            TextureManager.Instance.Draw($"{TextureId}Hull", Position, TextureScale, Rotation, TextureDepth, new(30, 30, 35));
            TextureManager.Instance.Draw($"{TextureId}Structure", Position, TextureScale, Rotation, TextureDepth, new(20, 20, 50));

            Defense.Draw(this);
        }
    }
}