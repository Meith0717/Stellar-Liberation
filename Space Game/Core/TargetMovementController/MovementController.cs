using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Game.GameLogik;
using Microsoft.Xna.Framework;
using System;

namespace Galaxy_Explovive.Core.MovementController
{
    public static class MovementController
    {

        public static Vector2? GetTargetPosition(SpatialHashing<GameObject.GameObject> spatialHashing, Vector2 worldMousePosition)
        {
            var gameObject = ObjectLocator.GetObjectsInRadius<InteractiveObject>(spatialHashing, worldMousePosition, 1500);
            if (gameObject.Count == 0) { return null; }
            //if (!gameObject[0].IsHover) { return null; }
            return gameObject[0].Position;
        }

        public static float GetRotation(float objectRotation,float targetRotation, float rotationRest)
        {
            float correction = 0;
            if (rotationRest < 0.05) { return targetRotation-objectRotation; }

            switch(objectRotation)
            {
                case >= 2 * MathF.PI: correction -= 2 * MathF.PI; break;
                case < 0: correction += 2 * MathF.PI; break; 
            }

            if (objectRotation < targetRotation && targetRotation - objectRotation <= MathF.PI) { correction += 0.05f; }
            if (objectRotation > targetRotation && objectRotation - targetRotation > MathF.PI) { correction += 0.05f; }
            if (objectRotation < targetRotation && targetRotation - objectRotation > MathF.PI) {  correction -= 0.05f; }
            if (objectRotation > targetRotation && objectRotation - targetRotation <= MathF.PI) { correction -= 0.05f; }

            return correction;
        }

        public static void GetVelocity(ref float currentVelovity, float maxVelocity, float targetDistance, float totalDistance, float rotationRest)
        {
            switch(rotationRest) 
            {
                case > 0.1f:
                    currentVelovity = 0.1f;
                    break;

                case < 0.1f:
                    switch (totalDistance)
                    {
                        case < 10000:
                            currentVelovity = 0.2f;
                            break;

                        case > 1000:
                            var procentualDistance = totalDistance * 0.1f;
                            procentualDistance = (procentualDistance > 10000) ? 10000 : procentualDistance;
                            if (targetDistance > totalDistance - procentualDistance)
                            {
                                currentVelovity = maxVelocity * ((totalDistance - targetDistance) / procentualDistance);
                                currentVelovity = (currentVelovity < 0.1f) ? 0.1f : currentVelovity;
                                break;
                            }
                            if (targetDistance < procentualDistance)
                            {
                                currentVelovity = maxVelocity * (targetDistance / procentualDistance);
                                break;
                            }
                            break;
                    }
                    break;
            }            
        }
    }
}
