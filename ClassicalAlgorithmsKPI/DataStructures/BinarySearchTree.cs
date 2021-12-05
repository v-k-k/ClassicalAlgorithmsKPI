using ClassicalAlgorithmsKPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassicalAlgorithmsKPI.DataStructures
{
    public class BinarySearchTree
    {
        public TreeNode<int> Root;
        public List<TreeNode<int>> Nodes;

        private int[] valuesContainer;

        private List<TreeNode<int>> CreatePreOrderTreeStructure(int[] source)
        {
            List<TreeNode<int>> nodesList = new List<TreeNode<int>>();
            TreeNode<int> parent = null;
            foreach (int value in source)
            {
                var node = new TreeNode<int>(value);
                node.Parent = parent;
                if (value == 0)
                    parent = node.Parent;
                else
                    parent = node;
                if (node.Parent != null)
                {
                    bool flag = true;
                    while (flag)
                    {
                        if (node.Parent.Left == null)
                        {
                            node.Parent.Left = node;
                            flag = false;
                        }
                        else if (node.Parent.Right == null)
                        {
                            node.Parent.Right = node;
                            flag = false;
                        }
                        else
                            node.Parent = node.Parent.Parent;
                    }
                }
                nodesList.Add(node);
            }
            return nodesList;
        }

        public BinarySearchTree(int[] preOrderSource)
        {
            int rootValue = preOrderSource[0];
            int[] sortedCleanedSource = preOrderSource.Where(item => item != 0).ToArray();
            Array.Sort(sortedCleanedSource);

            List<TreeNode<int>> preOrderNodes = CreatePreOrderTreeStructure(preOrderSource);
            Nodes = preOrderNodes[0].TraverseInOrder().ToList();
            valuesContainer = Nodes.Select(item => item.Value).ToArray();

            int rootValueIndex = Array.IndexOf(valuesContainer, rootValue);

            int[] sortedValuesContainer = (int[])valuesContainer.Clone();
            int valuesContainerIndex = 0;
            for (int i = 0; i < sortedValuesContainer.Length; i++)
            {
                if (sortedValuesContainer[i] != 0)
                {
                    sortedValuesContainer[i] = sortedCleanedSource[valuesContainerIndex];
                    valuesContainerIndex++;
                }
            }
            for (int i = 0; i < sortedValuesContainer.Length; i++)
                Nodes[i].Value = sortedValuesContainer[i];
            Root = Nodes.Where(item => item.Parent == null).First();
            Console.Write(1);
        }

        private void GetLeftBranchValues(TreeNode<int> startPoint, ref List<int> leftContainer)
        {
            leftContainer.Add(startPoint.Value);
            if (startPoint.Left != null && startPoint.Left.Value != 0)
                GetLeftBranchValues(startPoint.Left, ref leftContainer);
        }

        private void GetRightBranchValues(TreeNode<int> startPoint, ref List<int> rightContainer)
        {
            rightContainer.Add(startPoint.Value);
            if (startPoint.Right != null && startPoint.Right.Value != 0)
                GetRightBranchValues(startPoint.Right, ref rightContainer);
        }

        public int CountMonotonicSumms(int desiredSumm, out List<int>[] allWays)
        {
            HashSet<List<int>> uniqSuccessfullWays = new HashSet<List<int>>(new SequenceComparer<int>());
            HashSet<List<int>> foundMonotonicWays = new HashSet<List<int>>(new SequenceComparer<int>());
            List<List<int>> allBranches = new List<List<int>>();
            int counter = Nodes.Where(item => item.Value == desiredSumm).Count();
            var startPoint = Root;
            foreach (TreeNode<int> node in Nodes)
            {
                if (node == Root)
                    Console.Write(1);
                if (node.Value != 0)
                {
                    List<int> leftCounterContainer = new List<int>();
                    List<int> rightCounterContainer = new List<int>();
                    GetLeftBranchValues(node, ref leftCounterContainer);
                    GetRightBranchValues(node, ref rightCounterContainer);

                    allBranches.Add(leftCounterContainer);
                    allBranches.Add(rightCounterContainer);                  
                }
            }

            List<List<int>> complexBranches = new List<List<int>>();
            for (int i = 0; i < allBranches.Count; i++)
            {
                for (int j = 0; j < allBranches.Count; j++)
                {
                    if (i != j && allBranches[i].Count > 1 && allBranches[j].Count > 1)
                    {
                        var bothContain = allBranches[i].Intersect(allBranches[j]).ToList();
                        if (bothContain.Count > 0)
                        {
                            foreach (int sharedValue in bothContain)
                            {
                                if (allBranches[j].IndexOf(sharedValue) < allBranches[j].Count - 1)
                                {
                                    List<int> tmp = new List<int>();
                                    tmp.AddRange(allBranches[i].GetRange(
                                        0, allBranches[i].IndexOf(sharedValue) + 1));
                                    tmp.AddRange(allBranches[j].GetRange(
                                        allBranches[j].IndexOf(sharedValue) + 1,
                                        allBranches[j].Count - allBranches[j].IndexOf(sharedValue) - 1));
                                    complexBranches.Add(tmp);
                                }
                                if (allBranches[i].IndexOf(sharedValue) < allBranches[i].Count - 1)
                                {
                                    List<int> tmp = new List<int>();
                                    tmp.AddRange(allBranches[j].GetRange(
                                        0, allBranches[j].IndexOf(sharedValue) + 1));
                                    tmp.AddRange(allBranches[i].GetRange(
                                        allBranches[i].IndexOf(sharedValue) + 1, 
                                        allBranches[i].Count - allBranches[i].IndexOf(sharedValue) - 1));
                                    complexBranches.Add(tmp);
                                }
                            }
                        }
                    }
                }
            }
            allBranches.AddRange(complexBranches);

            foreach (List<int> monotonicBranch in allBranches.Where(item => item.Sum() >= desiredSumm).ToList())
                foundMonotonicWays.Add(monotonicBranch);

            foreach (List<int> way in foundMonotonicWays)
            {
                for (int valueIndex = 0; valueIndex < way.Count; valueIndex++)
                {
                    if (way[valueIndex] < desiredSumm && valueIndex + 1 < way.Count)
                    {
                        int start = valueIndex + 1;
                        List<int> tmpContainer = new List<int>() { way[valueIndex], way[start] };
                        while (tmpContainer.Sum() < desiredSumm && ++start < way.Count)
                            tmpContainer.Add(way[start]);
                        if (tmpContainer.Sum() == desiredSumm)
                            uniqSuccessfullWays.Add(tmpContainer);
                    }
                }
            }
            counter += uniqSuccessfullWays.Count;
            var almostAllWays = uniqSuccessfullWays.ToList();
            foreach (TreeNode<int> node in Nodes.Where(item => item.Value == desiredSumm))
                almostAllWays.Add(new List<int>() { node.Value });
            allWays = almostAllWays.ToArray();

            return counter;
        }
    }
}
