﻿using Galaxy_Explovive.Core.InputManagement;
using System;


namespace Galaxy_Explovive.Core.MovementController
{
    public class MovementController
    {
        private static MovementController mInstance = null;
        public static MovementController Instance { get { return mInstance ??= new MovementController(); } }

        public float GetRotation(float objectRotation,float targetRotation, float rotationRest)
        {
            float correction = 0;
            if (rotationRest < 0.1) { return targetRotation-objectRotation; }
            if (objectRotation >= 2 * MathF.PI) { correction -= 2 * MathF.PI; }
            if (objectRotation < 0) { correction += 2 * MathF.PI; }

            if (objectRotation < targetRotation && targetRotation - objectRotation <= MathF.PI) { correction += 0.05f; }
            if (objectRotation > targetRotation && objectRotation - targetRotation > MathF.PI) { correction += 0.05f; }
            if (objectRotation < targetRotation && targetRotation - objectRotation > MathF.PI) {  correction -= 0.05f; }
            if (objectRotation > targetRotation && objectRotation - targetRotation <= MathF.PI) { correction -= 0.05f; }

            return correction;
        }

        public float GetVelocity(InputState inputState, bool isSelect, float maxVelocity, float currentVelovity, float distanceToTarget)
        {
            if (distanceToTarget <= 40)
            {
                return -currentVelovity;
            }

            if (!isSelect) { return 0; }

            float ret = 0;
            float updateVelocity = maxVelocity * .005f;

            if (inputState.mActionList.Contains(ActionType.Deaccelerate))
            {
                if (currentVelovity - updateVelocity > Globals.SubLightVelocity)
                {
                    ret -= updateVelocity;
                } else
                {
                    ret -= currentVelovity - Globals.SubLightVelocity;
                }
            }
            if (inputState.mActionList.Contains(ActionType.Accelerate))
            {
                if (currentVelovity + updateVelocity < maxVelocity)
                {
                    ret += updateVelocity;
                } else
                {
                    ret += maxVelocity - currentVelovity;
                }
            }
            if (inputState.mActionList.Contains(ActionType.Stop))
            {
                ret = 0;
                ret -= currentVelovity;
            }
            return ret;
        }
    }
}