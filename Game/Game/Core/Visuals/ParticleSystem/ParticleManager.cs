// ParticleManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.


// ParticleManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.GameObjectManagement;

namespace StellarLiberation.Game.Core.Visuals.ParticleSystem
{
    public class ParticleManager : GameObjectManager
    {
        public void AddParticle(Particle particle) => AddObj(particle);

    }
}
