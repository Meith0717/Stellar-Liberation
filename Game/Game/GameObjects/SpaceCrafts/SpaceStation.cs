// SpaceStation.cs 
// Copyright (c) 2023-2024 Thierry Meiers 
// All rights reserved.

using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using StellarLiberation.Game.Core.GameProceses;
using StellarLiberation.Game.GameObjects.Spacecrafts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StellarLiberation.Game.GameObjects.SpaceCrafts
{
    [Serializable]
    public class SpaceStation : Spacecraft
    {
        [JsonProperty] public readonly SpaceStationID SpaceStationID;

        public SpaceStation(SpaceStationID spaceStationID, Vector2 position, Fractions fraction, string textureID, float textureScale, Vector2 engineTrailPosition) 
            : base(position, fraction, textureID, textureScale, Vector2.Zero) 
            => SpaceStationID = spaceStationID;
    }
}
