using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Maps;
using Google.Maps.Coord;
using Google.Maps.Event;
using Google.Maps.Feature.Style;
using Google.Maps.Unity.Intersections;

namespace BruteDrive.StageGeneration
{
    public sealed class RouteGenerator : MonoBehaviour
    {

        [Tooltip("The service provider for the maps API.")]
        [SerializeField] private MapsService mapsService = default;

        [SerializeField] private float desiredDistance = 300f;

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
