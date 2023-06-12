using Galaxy_Explovive.Core.GameObject;
using Galaxy_Explovive.Core.PositionManagement;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game.GameLogik;
using Galaxy_Explovive.Game.GameObjects.Spacecraft.SpaceShips;
using Microsoft.Xna.Framework;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Galaxy_Explovive.Core.TargetMovementController
{
    public class Movement
    {
        public float Velocity;
        public float Angle;

        public override string ToString()
        {
            return $"class Movement(Velocity: {Velocity}, Angle: {Angle})";
        }
    }

    public class MovementController
    {
        const int minDistanceToTarget = 100;
        const float OptimalAngle = 0.1f;
        private GameObject.GameObject mSourceObject;
        private float mDistanceToTarget;
        private float mTotalDistance;
        private float mAngleToTarget;
        private float mTargetAngle;
        private Movement mMovement = new();

        public MovementController(GameObject.GameObject sourceObject)
        {
            mSourceObject = sourceObject;
        }

        public Movement GetMovement() { System.Diagnostics.Debug.WriteLine(mMovement.ToString()); return mMovement;  }

        /// <summary> Returns False if Target is reached. Returns True if not. </summary>
        public bool MoveToTarget(GameObject.GameObject targetObject, Vector2 originPosition, 
            float rotation, float velocity, float maxVelocity, bool stop=false)
        {            
            // Define moving and target Positions
            Vector2 movingPosition = mSourceObject.Position;
            Vector2 targetPosition = targetObject.BoundedBox.ClosestPointTo(movingPosition);

            // Defining some helpfull Variables
            mDistanceToTarget = Vector2.Distance(movingPosition, targetPosition);
            mTotalDistance = Vector2.Distance(originPosition, targetPosition);
            mTargetAngle = MyUtility.GetAngle(movingPosition, targetPosition);
            mAngleToTarget = MathF.Abs(rotation - mTargetAngle);

            // Check if Destination is reached or velocity is zero
            if (mDistanceToTarget <= minDistanceToTarget || velocity <= 0) { return false; }
            
            mMovement.Angle = RotationController(rotation);
            mMovement.Velocity = VelocityController(velocity, maxVelocity, stop);
            return true;
        }

        private float VelocityController(float velocity, float maxVelocity, bool stop)
        {
            const float slowDownDistance = 1000;
            if (stop)
            {
                return (velocity <= 0.1f) ? 0 : velocity / 2;
            }
                switch (mAngleToTarget)
            {
                case > OptimalAngle: // Need To Correct angle
                    return 0.1f;

                case <= OptimalAngle: // Angle is Optimal

                    switch (mTotalDistance)
                    {
                        case < 10000: // To close to Target
                            return 0.2f;

                        case > 1000: // Far from Target (Need to Speed Up / Slow Down)

                            if (mDistanceToTarget < slowDownDistance) // Slow Down if approaching to Target
                            {
                                return (velocity <= 0.1f) ? 0 : 
                                    (maxVelocity * (mDistanceToTarget / slowDownDistance));
                            }

                            if (velocity < maxVelocity) // Speed up
                            {
                                return velocity * 2;
                            }
                            
                            return maxVelocity; // Traveling
                    }
                    break;
            }
            return 0;
        }
        private float RotationController(float angle)
        {
            if (mAngleToTarget <= OptimalAngle) { return mTargetAngle; }

            float correction = 0;
            switch (angle)
            {
                case >= 2 * MathF.PI: correction -= 2 * MathF.PI; break;
                case < 0: correction += 2 * MathF.PI; break;
            }

            if (angle < mTargetAngle && mTargetAngle - angle <= MathF.PI) { correction += 0.05f; }
            if (angle > mTargetAngle && angle - mTargetAngle > MathF.PI) { correction += 0.05f; }
            if (angle < mTargetAngle && mTargetAngle - angle > MathF.PI) { correction -= 0.05f; }
            if (angle > mTargetAngle && angle - mTargetAngle <= MathF.PI) { correction -= 0.05f; }

            return angle + correction;
        }

        public static GameObject.GameObject? SelectTargetObject(SpaceShip spaceShip,
            SpatialHashing<GameObject.GameObject> spatialHashing, Vector2 worldMousePosition)
        {
            var gameObject = ObjectLocator.GetObjectsInRadius<InteractiveObject>(spatialHashing, worldMousePosition, 1500);
            gameObject.Remove(spaceShip);
            if (gameObject.Count == 0) { return null; }
            if (!gameObject[0].IsHover) { return null; }
            return gameObject[0];
        }
    }
}
