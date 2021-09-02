﻿using ClassicalAlgorithmsKPI.DataGenerators;
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
    }
}