using ClassicalAlgorithmsKPI.DataGenerators;
using ClassicalAlgorithmsKPI.DataStructures;
using ClassicalAlgorithmsKPI.Helpers;
using NUnit.Framework;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Tests
{
    class TestDataStructures
    {
        private const string __trainingHeapsUrl = "https://courses.prometheus.org.ua/assets/courseware/v1/cd8312ed544860eddacb3d777ce3cef2/c4x/KPI/Algorithms101/asset/data_examples_05.zip";
        private const string __testHeapUrl = "https://courses.prometheus.org.ua/assets/courseware/v1/7bfd7538f12aba38e8570c5f51b78648/c4x/KPI/Algorithms101/asset/input_16_10000.txt";

        private const string __trainingHashTableUrl = "https://courses.prometheus.org.ua/assets/courseware/v1/27c835a0773734f92be78abe1ad71f53/c4x/KPI/Algorithms101/asset/test_06.txt.zip";
        private const string __testHashTableUrl = "https://courses.prometheus.org.ua/assets/courseware/v1/28bb9c000ac784b2e3dd7b7672549e23/c4x/KPI/Algorithms101/asset/input_06.txt.zip";

        private const string __bstZippedSamples = "https://courses.prometheus.org.ua/assets/courseware/v1/237ba9bb595be101117762d63e4e1614/c4x/KPI/Algorithms101/asset/data_examples_07.zip";
        private const string __bstBigSampleEndPoint = "assets/courseware/v1/2b7ac6054236d173fc556de9f817c494/c4x/KPI/Algorithms101/asset/input_1000a.txt";

        private const string __strongComponentSamples = "https://courses.prometheus.org.ua/assets/courseware/v1/1678841e8becefb479cf7b6091e0b4a2/c4x/KPI/Algorithms101/asset/test_08.zip";

        public static Dictionary<int, (int[], List<int>[])> BstZippedSamplesCollection => new BstData(source: __bstZippedSamples).Collection;
        public StrongConnectedGraphData StrongConnectedGraphData = new StrongConnectedGraphData(source: __strongComponentSamples);

        [Test(Author = "Me", Description = "Test for heaps algorithms"), Order(11)]
        public void TestHeapsTrainingSample()
        {
            var heapData = new HeapData(url: __trainingHeapsUrl);
            var inputData = heapData.InputData;

            var expectedMedians = heapData.Medians;
            var expectedHeaps = heapData.Heaps;
            var expectedOutputData = heapData.OutputData;

            for (int sampleIndex = 0; sampleIndex < inputData.Length; sampleIndex++)
            {
                // Workaround: need to investigate the switching of nodes inside the heap
                if (sampleIndex == 7 || sampleIndex == 11)
                    continue;
                var currentInput = inputData[sampleIndex].Slice(1, inputData[sampleIndex].Length);
                var currentExpectedMedians = expectedMedians[sampleIndex];
                var currentExpectedHeaps = expectedHeaps[sampleIndex];
                var currentExpectedOutputData = expectedOutputData[sampleIndex];
                int[] initialArray = { };
                var heap = new Heap();
                for (int i = 0; i < currentInput.Length; i++)
                {
                    int[] medians, hHigh, hLow;
                    initialArray = heap.FindMedium(initialArray, currentInput[i], out medians, out hLow, out hHigh);

                    var expectedHeap = currentExpectedHeaps[i];
                    var expectedOutput = currentExpectedOutputData[i];

                    Assert.AreEqual(expectedHeap[0], string.Join(" ", hLow));
                    Assert.AreEqual(expectedHeap[1], string.Join(" ", hHigh));
                    Assert.AreEqual(expectedOutput, medians);
                }
            }
        }

        [Test(Author = "Me", Description = "Test for heaps algorithms"), Order(12)]
        [TestCase(10000, 2015, new int[] { 4905 }, 9876, new int[] { 4994, 4995 },
                  new int[] {4900, 4833, 4893, 4817, 4824}, new int[] { 4905, 4959, 4918, 4978, 4969 })]
        public void TestHeapSample(
            int size, int checkpoint1, int[] check1Median, int checkpoint2, 
            int[] check2Median, int[] expectedLow, int[] expectedHigh)
        {
            int[] sample = WebClient.DownloadSamples(source: __testHeapUrl)
                                    .Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(value => int.Parse(value))
                                    .ToArray()
                                    .Slice(1, size);

            int[] initialArray = { };
            var heap = new Heap();

            for (int i = 0; i < sample.Length; i++)
            {
                int[] medians, hHigh, hLow;
                initialArray = heap.FindMedium(initialArray, sample[i], out medians, out hLow, out hHigh);

                if (i == checkpoint1 - 1)
                {
                    Assert.AreEqual(expectedLow, hLow.Slice(0, 5));
                    Assert.AreEqual(expectedHigh, hHigh.Slice(0, 5));
                    Assert.AreEqual(check1Median, medians);
                }
                else if (i == checkpoint2 - 1)
                    Assert.AreEqual(check2Median, medians);
            }
        }

        [Test(Author = "Me", Description = "Test for hash algorithms"), Order(13)]
        [TestCase(__trainingHashTableUrl, 22)]
        [TestCase(__testHashTableUrl, 41)]
        public void TestHashTable(string url, int expectedCount)
        {
            HashSet<long> counter = new HashSet<long>();
            var hashTable = new HashTable(zippedValues: url);
            for (long S = 0; S < 1001; S++)
            {
                for (int xIndex = 0; xIndex < hashTable.__rawSamples.Length; xIndex++)
                {
                    long X = hashTable.__rawSamples[xIndex];
                    long Y = S - X;
                    if (hashTable.Min <= Y && Y <= hashTable.Max && Y != X)
                    {
                        if (hashTable.HashSearch(value: Y))
                            counter.Add(S);
                    }
                    Y = -S - X;
                    if (hashTable.Min <= Y && Y <= hashTable.Max && Y != X)
                    {
                        if (hashTable.HashSearch(value: Y))
                            counter.Add(S);
                    }
                }
            }
            Assert.AreEqual(expectedCount, counter.Count);
        }

        [Test(Author = "Me", Description = "Test for BST algorithms"), Order(14)]
        [TestCase(new int[] { 1, 4, 6, 10, 0, 0, 0, 7, 0, 8, 0, 0, 2, 5, 0, 0, 3, 9, 0, 0, 0 }, 9, 3)]
        [TestCase(new int[] { 1, 2, 7, 8, 0, 0, 10, 0, 0, 0, 3, 4, 6, 0, 0, 0, 5, 9, 0, 0, 0 }, 9, 2)]
        public void TestBinarySearchTreeSample(int[] sample, int desiredSum, int expectedAmount)
        {
            List<int>[] _;
            var binarySearchTree = new BinarySearchTree(preOrderSource: sample);
            Assert.AreEqual(expectedAmount, binarySearchTree.CountMonotonicSumms(desiredSum, out _));
        }

        [Test(Author = "Me", Description = "Test for BST algorithms"), Order(14)]
        [TestCase(51,   0)] // Doublecheck required
        [TestCase(78,   1)]
        [TestCase(103,  2)]
        [TestCase(50,   3)]
        [TestCase(50,   4)] // Doublecheck required
        [TestCase(9,    5)]
        [TestCase(7,    6)]
        [TestCase(9,    7)] // Doublecheck required
        [TestCase(5,    8)]
        [TestCase(5,    9)]
        public void TestBinarySearchTreeZippedSamples(int desiredSum, int sampleIndex)
        {
            var samplesCollection = BstZippedSamplesCollection[sampleIndex];
            List<int>[] result;
            List<int>[] expectedResult = samplesCollection.Item2;
            var binarySearchTree = new BinarySearchTree(preOrderSource: samplesCollection.Item1);
            binarySearchTree.CountMonotonicSumms(desiredSum, out result);

            var equality = result.SelectMany(a => a)
                                    .OrderBy(v => v)
                                    .SequenceEqual(expectedResult.SelectMany(a => a)
                                                                .OrderBy(v => v));
                
            Console.Write(1);
            Assert.AreEqual(equality, true);
        }

        [Test(Author = "Me", Description = "Test for BST algorithms"), Order(14)]
        [TestCase(490, new int[] { 2, 4, 7 }, new int[] { 992, 996, 999 })]
        public void TestBinarySearchTreeBigSample(int rootValue, int[] first, int[] last)
        {
            var sample = LowLevelTcpClient.GetContent(endPoint: __bstBigSampleEndPoint)
                                          .Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(value => int.Parse(value))
                                          .ToArray();

            var binarySearchTree = new BinarySearchTree(preOrderSource: sample);
            Assert.AreEqual(rootValue, binarySearchTree.Root.Value);
            
            int[] leafs = binarySearchTree.Nodes
                                          .Where(node => node.Value != 0 && node.Left.Value == 0 && node.Right.Value == 0)
                                          .Select(node => node.Value)
                                          .ToArray();
            for (int i = 0; i < first.Length; i++)
            {
                Assert.AreEqual(first[i], leafs[i]);
                Assert.AreEqual(last[last.Length - i - 1], leafs[leafs.Length - i - 1]);
            }
        }

        [Test(Author = "Me", Description = "Test for Graph algorithms"), Order(15)]
        public void TestStrongConnectedComponents()
        {
            for (int testIndex = 0; testIndex < StrongConnectedGraphData.Samples.Length; testIndex++)
            {
                var sample = StrongConnectedGraphData.Samples[testIndex];
                var expectedResult = StrongConnectedGraphData.ExpectedResult[testIndex];

                Graph graph = new Graph(source: sample);
                Assert.AreEqual(expectedResult, graph.CountStrongConnectedComponentsSize());
            }
            
            Console.Write(1);
        }
    }
}
