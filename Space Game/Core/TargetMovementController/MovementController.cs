using Galaxy_Explovive.Core.GameLogik;
using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.InputManagement;
using Galaxy_Explovive.Core.Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaxy_Explovive.Core.MovementController
{
    public class MovementController
    {
        private static MovementController mInstance = null;
        public static MovementController Instance { get { return mInstance ??= new MovementController(); } }

        public float GetRotation(float objectRotation,float targetRotation, float rotationRest)
        {
            float correction = 0;
            if (objectRotation >= 2 * MathF.PI) { correction -= 2 * MathF.PI; }
            if (objectRotation < 0) { correction += 2 * MathF.PI; }

            if (objectRotation < targetRotation && targetRotation - objectRotation <= MathF.PI) { correction += 0.01f * rotationRest; }
            if (objectRotation > targetRotation && objectRotation - targetRotation > MathF.PI) { correction += 0.01f * rotationRest; }
            if (objectRotation < targetRotation && targetRotation - objectRotation > MathF.PI) {  correction -= 0.01f * rotationRest; }
            if (objectRotation > targetRotation && objectRotation - targetRotation <= MathF.PI) { correction -= 0.01f * rotationRest; }

            return correction;
        }

        public float GetVelocity(float maxVelocity, float currentVelovity, float rotationRest, float distanceToTarget)
        {
            float SubLightVelocity = Globals.SubLightVelocity;
            float updateVelocity = maxVelocity * .05f;
            if (distanceToTarget <= 40)
            {
                return -currentVelovity;
            }

            float ret = 0;
            if (distanceToTarget <= 2700 && currentVelovity >= SubLightVelocity + updateVelocity)
            {
                ret -= updateVelocity;
            }
            if ((rotationRest <= 0.01) && (currentVelovity - updateVelocity <= maxVelocity))
            {
                ret += updateVelocity;
            }
            return ret;
        }


    }
}
