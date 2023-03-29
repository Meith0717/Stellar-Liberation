using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Explovive.Game.GameObjects.Spacecraft
{
    public abstract class Spacecraft : InteractiveObject
    {

        public Vector2 TargetPosition { get; set; }

        public new void UpdateInputs(InputState inputState)
        {
            base.UpdateInputs(inputState);
        }
    }
}
