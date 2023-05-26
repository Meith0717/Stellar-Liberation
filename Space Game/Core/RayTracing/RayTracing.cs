using Galaxy_Explovive.Core.TextureManagement;
using Galaxy_Explovive.Core.Utility;
using Galaxy_Explovive.Game.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Galaxy_Explovive.Core.RayTracing
{
    public class RayTracer
    {
        private List<Ray> mRays = new();
        private Color mColor;
        private float mBorderLength;
        private List<GameObject.GameObject> mGameObjects;

        private int RayAmount;
        private int RayThickness;

        public RayTracer(Color color)
        {
            mColor = color;
        }

        public void GetRays(PlanetSystem ps)
        {
            // Clear all Lists
            mRays.Clear();

            if (!Globals.mRayTracing) { return; }

            var AmountUpdate = (2300 * 0.035f);
            RayAmount = (int)(2300 - (AmountUpdate / Globals.Camera2d.mZoom));
            if (RayAmount < 0 ) { RayAmount = 0; }
            RayThickness = 15 ;

            Vector2 scource = ps.Position;// Globals.mCamera2d.ViewToWorld(inS.mMousePosition.ToVector2());
            mBorderLength = ps.BoundedBox.Radius * 5;
            mGameObjects = GameLogik.ObjectLocator.Instance.GetObjectsInRadius(scource, (int)mBorderLength);
            mGameObjects.Remove(ps.mStar);

            // Create Rays around Scource
            for (float radiant = 0; radiant < (2 * MathF.PI); radiant += (MathF.PI * 2) / RayAmount)
            {
                // Create Ray
                Vector2 EndPosition = MyUtility.GetVector2(mBorderLength, radiant) + scource;
                Ray ray = new Ray(scource, EndPosition, radiant);

                // Add Ray with angle i to List
                mRays.Add(GetRayCollision(ray));
            }
        }

        private Ray GetRayCollision(Ray ray)
        {

            // Go throuth Planets
            foreach (GameObject.GameObject obj in mGameObjects)
            {
                // Get both Vector
                float objRadius = Vector2.Distance(obj.Position, ray.StartPosition);

                Vector2 rayPositionAtRadius = ray.GetPositionFromRadius(objRadius);

                // Get Distance and check if Ray pass throuth Planet
                if (Vector2.Distance(obj.Position, rayPositionAtRadius) < obj.BoundedBox.Radius)
                {
                    // Set Ray End to Ray Position at Planet Radius
                    ray.EndPosition = rayPositionAtRadius;
                    return ray;
                }
            }

            return ray;
        }

        public void Draw(TextureManager textureManager)
        {
            foreach (Ray ray in mRays)
            {
                textureManager.DrawLine(ray.StartPosition, ray.EndPosition, mColor, RayThickness, 0);
            }
        }
    }
}
