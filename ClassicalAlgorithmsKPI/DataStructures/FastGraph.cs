using ClassicalAlgorithmsKPI.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ClassicalAlgorithmsKPI.DataStructures
{
    public class FastGraph
    {
        private int __globalDfsCounter = 0;
        private HashSet<int> __visited = new HashSet<int>();
        private HashSet<int> __visitedTransponed = new HashSet<int>();
        private Dictionary<int, int> __dfsOrder = new Dictionary<int, int>();
        private Dictionary<int, int> __transponedDfsOrder = new Dictionary<int, int>();

        public Dictionary<int, List<int>> Nodes { get; } = new Dictionary<int, List<int>>();
        public Dictionary<int, List<int>> TransponedNodes { get; } = new Dictionary<int, List<int>>();
        public Dictionary<int, Dictionary<int, int>> WeightedEdges { get; } = new Dictionary<int, Dictionary<int, int>>();

        public FastGraph(int[][] source, bool weights = false)
        {
            foreach (int[] pair in source)
            {
                int nodeValue = pair[0];
                int siblingNodeValue = pair[1];
                if (weights)
                {
                    int edgeWeight = pair[2];
                    if (WeightedEdges.ContainsKey(nodeValue))
                    {
                        try
                        {
                            WeightedEdges[nodeValue].Add(siblingNodeValue, edgeWeight);
                        }
                        catch (System.ArgumentException)
                        { }
                    }
                    else
                        WeightedEdges.Add(nodeValue, new Dictionary<int, int> { { siblingNodeValue, edgeWeight } });

                    if (!WeightedEdges.ContainsKey(siblingNodeValue))
                        WeightedEdges.Add(siblingNodeValue, new Dictionary<int, int> ());
                }
                else
                {
                    int transponedNodeValue = pair[1];
                    int transponedSiblingNodeValue = pair[0];

                    if (Nodes.ContainsKey(nodeValue))
                        Nodes[nodeValue].Add(siblingNodeValue);
                    else
                        Nodes.Add(nodeValue, new List<int> { siblingNodeValue });

                    if (!Nodes.ContainsKey(siblingNodeValue))
                        Nodes.Add(siblingNodeValue, new List<int>());

                    if (TransponedNodes.ContainsKey(transponedNodeValue))
                        TransponedNodes[transponedNodeValue].Add(transponedSiblingNodeValue);
                    else
                        TransponedNodes.Add(
                            transponedNodeValue, new List<int> { transponedSiblingNodeValue });

                    if (!TransponedNodes.ContainsKey(transponedSiblingNodeValue))
                        TransponedNodes.Add(transponedSiblingNodeValue, new List<int>());
                }
            }
        }

        private int Dfs(int start, Dictionary<int, List<int>> container, HashSet<int> visited, Dictionary<int, int> dfsOrder)
        {
            Stack<int> stack = new Stack<int>();
            stack.Push(start);
            visited.Add(start);
            int componentsCounter = 1;
            while (stack.Count != 0)
            {
                int vertex = stack.Peek();
                int k = 0;
                if (container[vertex].Count > 0)
                {
                    while (visited.Contains(container[vertex][k]))
                    {
                        if (k == container[vertex].Count - 1)
                            break;
                        k++;
                    }
                    if (!visited.Contains(container[vertex][k]))
                    {
                        stack.Push(container[vertex][k]);
                        visited.Add(container[vertex][k]);
                        componentsCounter++;
                    }
                    else
                        dfsOrder.Add(stack.Pop(), __globalDfsCounter++);
                }
                else
                    dfsOrder.Add(stack.Pop(), __globalDfsCounter++);
            }
            return componentsCounter;
        }

        public int[] CountStrongConnectedComponentsSize()
        {
            List<int> componentsCollection = new List<int>();
            foreach (int node in Nodes.Keys)
            {
                if (!__visited.Contains(node))
                    Dfs(start: node, container: Nodes, visited: __visited, dfsOrder: __dfsOrder);
            }

            var keysInTransponedOrder = TransponedNodes.Keys
                                                       .OrderBy(gNode => __dfsOrder[gNode])
                                                       .Reverse()
                                                       .ToArray();

            foreach (int node in keysInTransponedOrder)
            {
                if (!__visitedTransponed.Contains(node))
                    componentsCollection.Add(
                        Dfs(start: node, container: TransponedNodes, visited: __visitedTransponed, dfsOrder: __transponedDfsOrder));
            }

            return componentsCollection.OrderBy(amount => amount)
                                       .Reverse()
                                       .ToArray();
        }

        public int[] Dijkstra(int start, int stopOn = int.MinValue)
        {
            HashSet<int> allVertexes = new HashSet<int>(WeightedEdges.Keys);
            Dictionary<int, int> Predecessors = allVertexes.ToDictionary(k => k, v => int.MinValue);
            Dictionary<int, double> Distances = allVertexes.ToDictionary(k => k, v => double.PositiveInfinity);
            Distances[start] = 0;

            HashSet<int> pointsPassed = new HashSet<int> { start };
            int vertex = start;
            while (!pointsPassed.SetEquals(allVertexes))
            {
                var remaining = new HashSet<int>(allVertexes);
                remaining.ExceptWith(pointsPassed);
                foreach (int siblingVertex in WeightedEdges[vertex].Keys)
                {
                    if (remaining.Contains(siblingVertex) && 
                        Distances[siblingVertex] > Distances[vertex] + WeightedEdges[vertex][siblingVertex])
                    {
                        Distances[siblingVertex] = Distances[vertex] + WeightedEdges[vertex][siblingVertex];
                        Predecessors[siblingVertex] = vertex;
                    }
                }

                int vertexMinValue = Distances.Where(pair => remaining.Contains(pair.Key))
                                              .MinBy(kvp => kvp.Value)
                                              .Key;
                pointsPassed.Add(vertexMinValue);
                vertex = vertexMinValue;
                if (vertex == stopOn)
                    break;
            }
            return Distances.OrderBy(pair => pair.Key)
                            .Select(pair => (int)pair.Value)
                            .ToArray();
        }
        
        public int[][] ShortestPathMatrix(int matrixSize)
        {
            int[][] result = new int[][] { };
            if (WeightedEdges.Count != matrixSize)
                for (int vertex = 1; vertex <= matrixSize; vertex++)
                    if (!WeightedEdges.Keys.Contains(vertex))
                        WeightedEdges.Add(vertex, new Dictionary<int, int> { { vertex, 0 } });

            for (int vertex = 1; vertex <= matrixSize; vertex++)
                result = result.Append(Dijkstra(start: vertex));

            return result;
        }

        public int[] OptimizedDijkstra(out Dictionary<int, int> counter, int start, int correctionAmount, int stopOn = int.MinValue)
        {
            if (WeightedEdges.Count != correctionAmount)
                for (int vert = 1; vert <= correctionAmount; vert++)
                    if (!WeightedEdges.Keys.Contains(vert))
                        WeightedEdges.Add(vert, new Dictionary<int, int> { { vert, 0 } });

            HashSet<int> allVertexes = new HashSet<int>(WeightedEdges.Keys);

            counter = allVertexes.ToDictionary(path => path, path => 0);
            var initQueue = allVertexes.Select(vert => new int[] { vert, int.MaxValue })
                                       .OrderBy(pair => pair[0])
                                       .ToList();

            initQueue[start - 1][1] = 0;
            counter[start] = 1;

            PriorityQueue priorityQueue = new PriorityQueue(initQueue);
            Dictionary<int, int> Predecessors = allVertexes.ToDictionary(k => k, v => int.MinValue);
            Dictionary<int, double> Distances = allVertexes.ToDictionary(k => k, v => double.PositiveInfinity);

            Distances[start] = 0;
            int vertex = start;

            while (priorityQueue.Count != 0)
            {
                vertex = priorityQueue.ExtractMin();
                foreach (int siblingVertex in WeightedEdges[vertex].Keys)
                {
                    if (Distances[siblingVertex] > Distances[vertex] + WeightedEdges[vertex][siblingVertex])
                    {
                        Distances[siblingVertex] = Distances[vertex] + WeightedEdges[vertex][siblingVertex];
                        Predecessors[siblingVertex] = vertex;
                        priorityQueue.DecreaseKey(siblingVertex, (int)Distances[siblingVertex]);
                        counter[siblingVertex] = counter[vertex];
                    }
                    else if (Distances[siblingVertex] == Distances[vertex] + WeightedEdges[vertex][siblingVertex])
                        counter[siblingVertex] += counter[vertex];
                }
                if (vertex == stopOn)
                    break;
            }
            
            return Distances.OrderBy(pair => pair.Key)
                            .Select(pair => (int)pair.Value)
                            .ToArray();
        }
    }
}
