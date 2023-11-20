// ParticleManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;

// ParticleManager.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using StellarLiberation.Game.Core.GameObjectManagement;
using StellarLiberation.Game.Core.InputManagement;
using StellarLiberation.Game.Core.LayerManagement;
using System.Collections.Generic;

namespace StellarLiberation.Game.Core.ParticleSystem
{
    public class ParticleManager : GameObjectManager
    {
        public void AddParticle(Particle particle) => AddObj(particle);

    }
}
