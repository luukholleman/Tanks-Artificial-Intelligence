﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Pathfinding
{
    class AStar
    {
        private readonly Graph _graph;

        public List<GraphNode> Path = new List<GraphNode>();

        private int _source;

        private int _target;

        Dictionary<int, int> _previous = new Dictionary<int, int>();
        Dictionary<int, float> _distances = new Dictionary<int, float>();
        public List<int> Closed = new List<int>();
        //PriorityQueue<int> pq = new PriorityQueue<int>();

        PriorityQueue pq = new PriorityQueue();

        int i = 0;

        private int _iterationsPerCall = 25;

        Stopwatch sw = new Stopwatch();
        public AStar(Graph graph, int source, int target)
        {
            _graph = graph;
            _source = source;
            _target = target;

            foreach (var vertex in _graph.GetNodes())
            {
                if (vertex.Index == _source)
                {
                    _distances[vertex.Index] = 0;
                }
                else
                {
                    _distances[vertex.Index] = float.MaxValue;
                }
            }

            pq.Enqueue(_source, 0);
        }

        public bool Search()
        {
            int j = 0;

            sw.Start();

            while (!pq.IsEmpty())
            {
                i++;

                var smallest = (int)pq.Dequeue();

                // we've found the path
                if (smallest == _target)
                {
                    while (_previous.ContainsKey(smallest))
                    {
                        Path.Add(_graph.GetNode(smallest));
                        smallest = _previous[smallest];
                    }

                    sw.Stop();

                    Debug.Log("Path found!!");
                    Debug.Log("Iterations: " + i);
                    Debug.Log(sw.Elapsed);

                    Path.Reverse();

                    return true;
                }

                // can't find a path
                if (_distances[smallest] == float.MaxValue)
                {
                    return false;
                }

                foreach (var neighbor in _graph.GetNode(smallest).Edges)
                {
                    var alternativeCost = _distances[smallest] + neighbor.Cost;

                    if (alternativeCost < _distances[neighbor.To])
                    {
                        _distances[neighbor.To] = alternativeCost;
                        _previous[neighbor.To] = smallest;
                    }

                    if (!Closed.Exists(n => n == neighbor.To))
                    {
                        float distance = Vector2.Distance(_graph.GetNode(_target).Position, _graph.GetNode(smallest).Position);
                        
                        pq.Enqueue(neighbor.To, (int)(alternativeCost + distance));
                    }

                    Closed.Add(neighbor.To);
                }

                if (++j >= _iterationsPerCall)
                {
                    return false;   
                }
            }

            return false;
        }
        //private bool Search()
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
        //    var previous = new Dictionary<int, int>();
        //    var distances = new Dictionary<int, float>();
        //    var nodes = new List<int>();
        //    var Closed = new List<int>();

        //    PriorityQueue<int> pq = new PriorityQueue<int>();

        //    foreach (var vertex in _graph.GetNodes())
        //    {
        //        if (vertex.Index == _source)
        //        {
        //            distances[vertex.Index] = 0;
        //        }
        //        else
        //        {
        //            distances[vertex.Index] = 900001;
        //        }

        //        //nodes.Add(vertex.Index);
        //    }

        //    //nodes.Add(_graph.GetNode(_source).Index);

        //    pq.Enqueue(new PriorityQueueNode<int>(0, _source));

        //    int i = 0;
        //    while (!pq.Empty())
        //    {
        //        i++;
        //        //nodes.Sort((x, y) => (int)(distances[x] - distances[y]));

        //        var smallest = pq.DeQueue().Value;

        //        //var smallest = nodes[0];
        //        //nodes.Remove(smallest);

        //        if (smallest == _target)
        //        {
        //            while (previous.ContainsKey(smallest))
        //            {
        //                Path.Add(_graph.GetNode(smallest));
        //                smallest = previous[smallest];
        //            }

        //            sw.Stop();
        //            Debug.Log("Path found!!");
        //            Debug.Log("Iterations: " + i);
        //            Debug.Log(sw.Elapsed);

        //            Path.Reverse();

        //            return true;
        //        }

        //        if (distances[smallest] == 900001)
        //        {
        //            return false;
        //        }

        //        foreach (var neighbor in _graph.GetNode(smallest).Edges)
        //        {
        //            float distance = Vector2.Distance(_graph.GetNode(_target).Position,
        //                _graph.GetNode(smallest).Position);

        //            var alt = distances[smallest] + neighbor.Cost;

        //            if (alt < distances[neighbor.To])
        //            {
        //                distances[neighbor.To] = alt;
        //                previous[neighbor.To] = smallest;
        //            }

        //            if (!Closed.Exists(n => n == neighbor.To))
        //                pq.Enqueue(new PriorityQueueNode<int>(alt + distance, neighbor.To));

        //            Closed.Add(neighbor.To);
        //        }
        //    }

        //    return false;
        //}
    }
}
