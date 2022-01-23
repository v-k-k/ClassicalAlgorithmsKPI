using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalAlgorithmsKPI.DataStructures
{
    public class GraphNode : IComparable<GraphNode>
    {
        public int Value;
        public int NumberInDfsOrder;
        public List<GraphNode> Edges = new List<GraphNode>();
        public bool Visited = false;

        public GraphNode(int value)
        {
            Value = value;
        }

        public override bool Equals(object other)
        {
            return other is GraphNode node
                && node.Value == this.Value;
        }

        public override int GetHashCode()
        {
            unchecked // Allow arithmetic overflow, numbers will just "wrap around"
            {
                int hashcode = 1430287;
                hashcode *= 7302013 ^ this.Value.GetHashCode();
                return hashcode * 7302013 ^ this.Value.GetHashCode();
            }
        }

        // Default comparer for GraphNode type.
        // A null value means that this object is greater.
        public int CompareTo(GraphNode node) =>
            node == null ? 1 : Value.CompareTo(node.Value);
    }
}
