using System.Collections.Generic;
using System.Linq;
using System;

namespace ClassicalAlgorithmsKPI.DataStructures
{
    public class Graph
    {
        private int __globalDfsCounter = 0;

        public SortedSet<GraphNode> Nodes { get; } = new SortedSet<GraphNode>();
        public SortedSet<GraphNode> TransponedNodes { get; } = new SortedSet<GraphNode>();

        public Graph(int[][] source)
        {
            foreach (int[] pair in source)
            {
                GraphNode node, siblingNode, transponedNode, transponedSiblingNode;

                int nodeValue = pair[0];
                int siblingNodeValue = pair[1];
                int transponedNodeValue = pair[1];
                int transponedSiblingNodeValue = pair[0];

                if (Nodes.Contains(new GraphNode(nodeValue)))
                    node = Nodes.Where(item => item.Value == nodeValue).First();
                else
                {
                    node = new GraphNode(nodeValue);
                    Nodes.Add(node);
                }

                if (Nodes.Contains(new GraphNode(siblingNodeValue)))
                    siblingNode = Nodes.Where(item => item.Value == siblingNodeValue).First();
                else
                {
                    siblingNode = new GraphNode(siblingNodeValue);
                    Nodes.Add(siblingNode);
                }
                node.Edges.Add(siblingNode);

                if (TransponedNodes.Contains(new GraphNode(transponedNodeValue)))
                    transponedNode = TransponedNodes.Where(item => item.Value == transponedNodeValue).First();
                else
                {
                    transponedNode = new GraphNode(transponedNodeValue);
                    TransponedNodes.Add(transponedNode);
                }

                if (TransponedNodes.Contains(new GraphNode(transponedSiblingNodeValue)))
                    transponedSiblingNode = TransponedNodes.Where(item => item.Value == transponedSiblingNodeValue).First();
                else
                {
                    transponedSiblingNode = new GraphNode(transponedSiblingNodeValue);
                    TransponedNodes.Add(transponedSiblingNode);
                }
                transponedNode.Edges.Add(transponedSiblingNode);
            }
        }

        private int Dfs(GraphNode start)
        {
            Stack<GraphNode> stack = new Stack<GraphNode>();
            stack.Push(start);
            start.Visited = true;
            int componentsCounter = 1;
            while (stack.Count != 0)
            {
                GraphNode vertex = stack.Peek();
                int k = 0;
                if (vertex.Edges.Count > 0)
                {
                    while (vertex.Edges[k].Visited)
                    {
                        if (k == vertex.Edges.Count - 1)
                            break;
                        k++;
                    }
                    if (!vertex.Edges[k].Visited)
                    {
                        stack.Push(vertex.Edges[k]);
                        vertex.Edges[k].Visited = true;
                        componentsCounter++; 
                    }
                    else
                    {
                        stack.Pop().NumberInDfsOrder = __globalDfsCounter++;
                    }
                }
                else
                {
                    stack.Pop().NumberInDfsOrder = __globalDfsCounter++;
                }
            }
            return componentsCounter;
        }

        public int[] CountStrongConnectedComponentsSize()
        {
            List<int> componentsCollection = new List<int>();
            foreach (GraphNode node in Nodes)
            {
                if (!node.Visited)
                    Dfs(start: node);
            }

            var transponedGraphOrder = Nodes.OrderBy(gNode => gNode.NumberInDfsOrder).Reverse().ToArray();

            foreach (GraphNode node in TransponedNodes.OrderBy(
                transponedNode => Array.IndexOf(transponedGraphOrder, transponedNode)))
            {
                if (!node.Visited)
                    componentsCollection.Add(Dfs(start: node));
            }

            return componentsCollection.OrderBy(amount => amount)
                                       .ToArray();
        }
    }
}
