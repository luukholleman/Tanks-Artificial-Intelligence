﻿using UnityEngine;

namespace Assets.Scripts.Goals.Evaluator
{
    abstract class GoalEvaluator
    {
        public Goal Goal;

        protected GameObject Instance;

        public void SetGameObject(GameObject instance)
        {
            Instance = instance;
        }

        public abstract float CalculateDesirability();

        public abstract Goal GetGoal();
    }
}
