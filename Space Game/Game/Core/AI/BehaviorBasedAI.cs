﻿
using CelestialOdyssey.Game.Core.GameObjects;
using CelestialOdyssey.Game.GameObjects.Spacecrafts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CelestialOdyssey.Game.Core.AI
{
    public class BehaviorBasedAI
    {
        private Behavior mCurrentBehavior;
        private HashSet<Behavior> mBehaviors;

        private int mCoolDown;

        public BehaviorBasedAI(HashSet<Behavior> behaviors)
        {
            mBehaviors = behaviors;
            mCoolDown = Utility.Utility.Random.Next(100);
        }

        public void AddBehavior(Behavior behavior)
        {
            mBehaviors.Add(behavior);
        }

        public void Update(GameTime gameTime, List<GameObject> environment, SpaceShip spaceShip)
        {
            mCoolDown -= gameTime.ElapsedGameTime.Milliseconds;
            if (mCoolDown > 0) return;
            mCoolDown = 500;

            // Auswahl des Verhaltens basierend auf Prioritäten und Spielsituation
            mCurrentBehavior = SelectBehavior(environment, spaceShip);

            // Ausführung des ausgewählten Verhaltens
            mCurrentBehavior?.Execute(environment, spaceShip);
        }

        private Behavior SelectBehavior(List<GameObject> environment, SpaceShip spaceShip)
        {
            PriorityQueue<Behavior, double> behaviors = new();
            foreach (var item in mBehaviors)
            {
                behaviors.Enqueue(item, - item.GetPriority(environment, spaceShip));
            }
            if (!behaviors.TryPeek(out var behavior, out var priority)) return null;
            if (priority == 0) return null;
            return behavior;
        }
    }
}