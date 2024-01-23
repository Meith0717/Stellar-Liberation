// AlliedShip.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using StellarLiberation.Game.Core.CoreProceses.InputManagement;
using StellarLiberation.Game.Core.CoreProceses.SceneManagement;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.PropulsionSystem;
using StellarLiberation.Game.Core.GameProceses.SpaceShipManagement.Systems.WeaponSystem;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts.SpaceShips.Allies
{
    public class AlliedShip : SpaceShip
    {

        private readonly TractorBeam mTractorBeam;

        public AlliedShip(Vector2 position, string textureId, float textureScale, SensorSystem sensorArray, SublightDrive sublightEngine, TurretSystem weaponSystem, DefenseSystem defenseSystem, TractorBeam tractorBeam) : base(position, textureId, textureScale, sensorArray, sublightEngine, weaponSystem, defenseSystem, Fractions.Allies, Color.LightSkyBlue, Color.Green) => mTractorBeam = tractorBeam;

        public override void Update(GameTime gameTime, InputState inputState, Scene scene)
        {
            base.Update(gameTime, inputState, scene);
            mTractorBeam.Pull(gameTime, this, scene);
        }

        public override void Draw(Scene scene)
        {
            base.Draw(scene);
            mTractorBeam.Draw(this);
        }
    }
}
