using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Maps;
using Google.Maps.Coord;
using Google.Maps.Event;
using Google.Maps.Feature.Style;
using Google.Maps.Unity.Intersections;
using BruteDrive.Utilities.Unity.Extensions;

namespace BruteDrive.StageGeneration
{
    public sealed class RouteGenerator : MonoBehaviour
    {
        public event Action Generated;


        [Tooltip("The service provider for the maps API.")]
        [SerializeField] private MapsService mapsService = default;

        [Tooltip("The object used to block unused paths.")]
        [SerializeField] private GameObject blockadePrefab = default;
        [SerializeField] private float blockadeDepth = 5f;

        [SerializeField] private float desiredDistance = 300f;

        public Vector3[] CruiserSpawnPoints { get; private set; }

        public RoadLatticeNode[] GenerateRoute()
        {
            int count = 0;
            foreach (RoadLatticeNode node in mapsService.RoadLattice.Nodes)
                count++;

            // Initialize collections for graph exploration.
            RoadLatticeNode[] bestPath = default;
            float bestPathLength = 0f;
            Stack<RoadLatticeNode> currentPath = new Stack<RoadLatticeNode>();
            float currentPathLength = 0f;
            // Initialize starting point for route finding.
            RoadLatticeNode current = mapsService.RoadLattice.SnapToNode(Vector2.zero);
            currentPath.Push(current);
            // Kick off recursive algorithm to find
            // a suitable path for the desired distance.
            ExploreJunctionsRecursive(current);

            // Now that we have the best path,
            // place some blockades along the path.
            for (int i = 0; i < bestPath.Length; i++)
            {
                RoadLatticeNode priorNode = (i > 0) ? bestPath[i - 1] : null;
                RoadLatticeNode nextNode = (i < bestPath.Length - 1) ? bestPath[i + 1] : null; 
                foreach (RoadLatticeNode node in bestPath[i].Neighbors)
                {
                    if (node != priorNode && node != nextNode)
                    {
                        // Place a blockade partway down this path.
                        Instantiate(blockadePrefab,
                            (bestPath[i].Location + ((node.Location - bestPath[i].Location).normalized * blockadeDepth)).TopDownUnflatten(),
                            Quaternion.LookRotation((bestPath[i].Location - node.Location).TopDownUnflatten()));
                    }
                }
            }

            Generated?.Invoke();
            return bestPath;

            bool ExploreJunctionsRecursive(RoadLatticeNode currentNode)
            {
                foreach (RoadLatticeNode node in currentNode.Neighbors)
                {
                    if (!currentPath.Contains(node))
                    {
                        // This path is not explored.
                        currentPath.Push(node);
                        float deltaLength = Vector2.Distance(currentNode.Location, node.Location);
                        currentPathLength += deltaLength;
                        // Will this path be a new record?
                        if (currentPathLength > bestPathLength)
                        {
                            // Document the new record.
                            bestPathLength = currentPathLength;
                            bestPath = currentPath.ToArray();
                            // If this path is long enough,
                            // then complete the search.
                            if (bestPathLength > desiredDistance)
                                return true;
                        }
                        // Keep exploring down this node.
                        if (ExploreJunctionsRecursive(node))
                            // Did somewhere deeper find a solution?
                            // If so bubble out of recursion.
                            return true;
                        // Finish exploring this branch.
                        currentPathLength -= deltaLength;
                        currentPath.Pop();
                    }
                }
                // Nothing down this path yielded
                // the desired path length.
                return false;
            }
        }
    }
}
