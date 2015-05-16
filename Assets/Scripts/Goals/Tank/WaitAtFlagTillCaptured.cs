﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Goals.Tank
{
    class WaitAtFlagTillCaptured : Goal
    {
        private GameObject _flag;

        private SteeringBehaviour _steeringBehaviour;

        private Rigidbody2D _rigidbody;

        public WaitAtFlagTillCaptured(GameObject flag)
        {
            _flag = flag;
        }

        public override void Activate()
        {
            Debug.Log("Waiting for flag " + _flag + " to capture");

            _steeringBehaviour = SteeringBehaviour.CreateInstance<SteeringBehaviour>();

            _steeringBehaviour.SetGameObject(Instance);

            _rigidbody = Instance.GetComponent<Rigidbody2D>();
        }

        public override STATUS Process()
        {
            _rigidbody.AddForce(_steeringBehaviour.Stop(_rigidbody.velocity, 0.5f));

            if (_flag.GetComponent<Flag>().Side == Instance.GetComponent<Vehicle>().Side)
            {
                return SetStatus(STATUS.COMPLETED);
            }

            if (Vector2.Distance(Instance.transform.position, _flag.transform.position) > _flag.GetComponent<Flag>().CappingRange)
            {
                // somehow we got out of the range of the flag (probably shot out), we need to get back in range
                if ((SubGoals.Any() && SubGoals.Peek().GetType() != typeof(CaptureFlag)) || !SubGoals.Any())
                    AddSubGoal(new CaptureFlag(_flag));

                return ProcessSubGoals();
            }

            return SetStatus(STATUS.ACTIVE);
        }

        public override void Terminate()
        {
            Debug.Log("Flag " + _flag + " is captured");
        }

        public override bool HandleMessage()
        {
            return true;
        }
    }
}
