using System;
using System.Collections.Generic;

namespace ClassicalAlgorithmsKPI.DataStructures
{
    public class TreeNode<T> where T : IComparable<T>
    {
        private const int EMPTY_NODE = 0;

        public T Value { get; set; }
        public TreeNode<T> Left { get; set; }
        public TreeNode<T> Right { get; set; }
        public TreeNode<T> Parent { get; set; }

        public TreeNode(T value)
        {
            Value = value;
        }

        public TreeNode<T> Get(T value)
        {
            int compare = value.CompareTo(Value);
            if (compare == 0)
                return this;

            if (compare < 0)
            {
                if (Left != null)
                    return Left.Get(value);
            }
            else
            {
                if (Right != null)
                    return Right.Get(value);
            }

            return null;
        }

        public IEnumerable<TreeNode<T>> TraverseInOrder()
        {
            var list = new List<TreeNode<T>>();
            InnerTraverse(list);
            return list;
        }

        private void InnerTraverse(List<TreeNode<T>> list)
        {
            if (Left != null)
                Left.InnerTraverse(list);
            
            list.Add(this);

            if (Right != null)
                Right.InnerTraverse(list);
        }

        public IEnumerable<T> TraversePreOrder()
        {
            var list = new List<T>();
            OuterTraverse(list);
            return list;
        }

        private void OuterTraverse(List<T> list)
        {
            list.Add(Value);

            if (Left != null)
                Left.OuterTraverse(list);
            
            if (Right != null)
                Right.OuterTraverse(list);
        }

        public T Min()
        {
            if (Left != null)
                return Left.Min();
            return Value;
        }
        public T Max()
        {
            if (Right != null)
                return Right.Max();
            return Value;
        }
    }
}
