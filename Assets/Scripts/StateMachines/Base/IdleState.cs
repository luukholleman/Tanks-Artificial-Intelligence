﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.StateMachines.Messaging;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.StateMachines.Base
{
    public class IdleState : IState
    {
        public Dictionary<float, Vector3> ToSpawn = new Dictionary<float, Vector3>();
    
        public override void Update(GameObject instance)
        {
            List<float> spawnedTanks = new List<float>();

            foreach (KeyValuePair<float, Vector3> pair in ToSpawn.Where(t => t.Key < Time.timeSinceLevelLoad))
            {
                GameObject tank = Resources.Load<GameObject>("PreFabs/Tank");

                GameObject closestFlag = FindClosestFlag(pair.Value);

                GameObject newTank = GameObject.Instantiate(tank, closestFlag.transform.position + new Vector3(Random.value * 10 - 5, Random.value * 10 - 5, 0).normalized, new Quaternion()) as GameObject;
                
                newTank.transform.parent = GameObject.Find("Tanks").transform;
                newTank.GetComponent<Tank.Tank>().Side = instance.GetComponent<Spawn>().Side;

                spawnedTanks.Add(pair.Key);
            }

            foreach (float f in spawnedTanks) ToSpawn.Remove(f);
        }

        public GameObject FindClosestFlag(Vector3 pos)
        {
            GameObject closest = null;
            float dist = float.MaxValue;

            foreach (Transform transform in GameObject.Find("Flags").transform)
            {
                float newDist = Vector3.Distance(pos, transform.position);
                if (newDist < dist && transform.GetComponent<Flag>().Side == Instance.GetComponent<Spawn>().Side)
                {
                    bool foundEnemy = false;

                    Collider2D[] tanks = Physics2D.OverlapCircleAll(transform.position,
                        transform.GetComponent<Flag>().CappingRange, LayerMask.GetMask("Tank"));

                    foreach (Collider2D tank in tanks)
                    {
                        if (tank.GetComponent<Tank.Tank>().Side != Instance.GetComponent<Spawn>().Side)
                        {
                            foundEnemy = true;
                            break;
                        }
                    }

                    if (!foundEnemy)
                    {
                        closest = transform.gameObject;
                        dist = newDist;
                    }
                }
            }

            // if all flags are of the enemy, spawn on the base
            if (closest == null)
                closest = Instance.gameObject;

            return closest;
        }

        public override void Enter(GameObject instance)
        {

        }

        public override void Exit(GameObject instance)
        {
        
        }

        public override void HandleMessage(GameObject instance, Message msg)
        {
            if (msg.Msg == Message.MessageType.TankDied)
            {
                if (msg.Sender.GetComponent<Tank.Tank>().Side == instance.GetComponent<Spawn>().Side)
                {
                    int i = 0;
                    while (true)
                    {
                        try
                        {
                            ToSpawn.Add(Time.timeSinceLevelLoad + Settings.Instance.TankSpawnDelay + i, msg.Sender.transform.position);
                            break;
                        }
                        catch (ArgumentException)
                        {
                            i++;
                        }
                    }
                }
            }
        }
    }
}
