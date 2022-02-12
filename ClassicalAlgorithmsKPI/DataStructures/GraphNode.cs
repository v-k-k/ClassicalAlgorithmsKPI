using System.Collections.Generic;
using System;

namespace ClassicalAlgorithmsKPI.DataStructures
{
    public class GraphNode : IComparable<GraphNode>
    {
        private int __hashCode;

        public int Value;
        public int NumberInDfsOrder;
        public List<GraphNode> Edges = new List<GraphNode>();
        public bool Visited = false;

        public GraphNode(int value)
        {
            Value = value;
            unchecked // Allow arithmetic overflow, numbers will just "wrap around"
            {
                __hashCode = value.GetHashCode();
            }
        }

        public override bool Equals(object other)
        {
            return other is GraphNode node
                && node.Value == this.Value;
        }

        public override int GetHashCode()
        {
            return __hashCode;
        }

        // Default comparer for GraphNode type.
        // A null value means that this object is greater.
        public int CompareTo(GraphNode node) =>
            node == null ? 1 : Value.CompareTo(node.Value);
    }
}
