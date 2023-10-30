// BehaviorBasedAI.cs 
// Copyright (c) 2023 Thierry Meiers 
// All rights reserved.

using CelestialOdyssey.Game.Core.SpaceShipManagement;
using CelestialOdyssey.Game.Core.SpaceShipManagement.ShipSystems;
using CelestialOdyssey.Game.Core.Utilitys;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.AI
{
    public class UtilityAi
    {
        private Behavior mCurrentBehavior;
        private Behavior mLastBehavior;
        private HashSet<Behavior> mBehaviors;

        private int mCoolDown;

        public UtilityAi(HashSet<Behavior> behaviors)
        {
            mBehaviors = behaviors;
            mCoolDown = ExtendetRandom.Random.Next(1000);
        }

        public void AddBehavior(Behavior behavior)
        {
            mBehaviors.Add(behavior);
        }

        public void Update(GameTime gameTime, SensorArray environment, SpaceShip spaceShip)
        {
            mCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            if (mCoolDown > 0) return;
            mCoolDown = 1000;

            // Auswahl des Verhaltens basierend auf Prioritäten und Spielsituation
            mCurrentBehavior = SelectBehavior(environment, spaceShip);

            if (mCurrentBehavior != mLastBehavior) mLastBehavior?.Reset();

            // Ausführung des ausgewählten Verhaltens
            mCurrentBehavior?.Execute(environment, spaceShip);
            mLastBehavior = mCurrentBehavior;
        }

        private Behavior SelectBehavior(SensorArray environment, SpaceShip spaceShip)
        {
            PriorityQueue<Behavior, double> behaviors = new();
            foreach (var item in mBehaviors) behaviors.Enqueue(item, -item.GetPriority(environment, spaceShip));
            if (!behaviors.TryPeek(out var behavior, out var priority)) return null;
            return behavior;
        }
    }
}
