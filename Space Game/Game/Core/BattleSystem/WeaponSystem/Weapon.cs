﻿using CelestialOdyssey.Core.GameEngine.Content_Management;
using CelestialOdyssey.Game.Core.BattleSystem.WeaponSystem.Projectiles;
using CelestialOdyssey.Game.Core.InputManagement;
using CelestialOdyssey.Game.Core.LayerManagement;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using CelestialOdyssey.GameEngine.Content_Management;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.BattleSystem.WeaponSystem
{
    public class PhotonTorpedo : Weapon
    {
        public PhotonTorpedo(Vector2 relativePosition) : base(relativePosition, 5000, "torpedoFire") { }

        public override void Fire(SpaceShip target, GameLayer gameLayer)
        {
            var projectile = new Torpedo(mPosition, target, ContentRegistry.photonTorpedo.Name, 2, 5);
            projectile.SetGameLayer(gameLayer);
            AddProjectile(projectile);
        }
    }

    public class PhotonPhaser : Weapon
    {
        public PhotonPhaser(Vector2 relativePosition) : base(relativePosition, 2000, "") { }

        public override void Fire(SpaceShip target, GameLayer gameLayer)
        {
            var projectile = new Phaser(mPosition, target, Color.BlueViolet, 2, 5);
            projectile.SetGameLayer(gameLayer);
            AddProjectile(projectile);
        }
    }

    public abstract class Weapon
    {
        private List<Projectile> mProjecvtiles = new();
        private int mMaxCoolDown;
        private int mCooldown;
        private string? mSound;
        private Vector2 mRelativePosition;
        internal Vector2 mPosition;

        internal Weapon(Vector2 relativePosition, int coolDownMs, string sound)
        {
            mCooldown = mMaxCoolDown = coolDownMs;
            mRelativePosition = relativePosition;
            mSound = sound;
        }

        public abstract void Fire(SpaceShip target, GameLayer gameLayer);

        internal virtual void AddProjectile(Projectile projectile)
        { 
            if (mCooldown < mMaxCoolDown) return; 
            mProjecvtiles.Add(projectile);
            mCooldown = 0;
            if (mSound == null) return;
            SoundManager.Instance.PlaySound(mSound, 1f);
        }

        public void Update(SpaceShip ship, GameTime gameTime, InputState inputState)
        {
            float cosTheta = (float)Math.Cos(ship.Rotation);
            float sinTheta = (float)Math.Sin(ship.Rotation);
            Vector2 rotatedVector = new Vector2(
                mRelativePosition.X * cosTheta - mRelativePosition.Y * sinTheta,
                mRelativePosition.X * sinTheta + mRelativePosition.Y * cosTheta
            );
            mPosition = rotatedVector + ship.Position;
            mCooldown += gameTime.ElapsedGameTime.Milliseconds;
                
            if (mProjecvtiles.Count == 0) return;
            List<Projectile> deleteList = new();
            foreach (var projectile in mProjecvtiles)
            {
                if (projectile.LiveTime <= 0)
                {
                    deleteList.Add(projectile);
                    continue;
                }
                projectile.Update(mPosition, gameTime, inputState);
            }

            foreach (var projectile in deleteList)
            {
                projectile.RemoveFromSpatialHashing();
                mProjecvtiles.Remove(projectile);
            }
        }
    }
}
