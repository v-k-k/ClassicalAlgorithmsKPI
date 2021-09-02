using NUnit.Framework;
using ClassicalAlgorithmsKPI.Sorting;
using System.Linq;
using System;
using ClassicalAlgorithmsKPI.DataGenerators;
using ClassicalAlgorithmsKPI.Helpers;
using System.Collections.Generic;

namespace Tests
{
    [TestFixture]
    public class TestSorting
    {
        private const string __trainingQuickSortingUrl = "https://courses.prometheus.org.ua/assets/courseware/v1/ae98a664c6552881a37593b6b7dd6782/c4x/KPI/Algorithms101/asset/data_examples_03.zip";
        private const string __quickSortUrl = "https://courses.prometheus.org.ua/assets/courseware/v1/31d63b32652e3d457f7b0c084b0019a8/c4x/KPI/Algorithms101/asset/input__10000.txt";
        private const string __radixSortUrl = "https://courses.prometheus.org.ua/assets/courseware/v1/c527b289b77bd5c7d2851ca728471685/c4x/KPI/Algorithms101/asset/anagrams.txt";
        static int[] __sample = { 4, 45, 66, 1, -45, 503, -40, 20, 993, 0, 11 };
        public int[] Sample => __sample;

        private int[] SortedSample => __sample.OrderBy(value => value).ToArray();

        [Test(Author = "Me", Description = "Test for insertion sort algorithm"), Order(2)]
        public void TestInsertionSort()
        {
            int[] generated = this.Sample;
            InsertionSort.SortInt(ref generated);
            Assert.AreEqual(generated, SortedSample);
        }

        [Test(Author = "Me", Description = "Test for merge sort algorithm"), Order(3)]
        public void TestMergeSort()
        {
            int[] generated = this.Sample;
            MergeSort.SortInt(ref generated);
            Assert.AreEqual(generated, SortedSample);
        }

        [Test(Author = "Me", Description = "Test for quick sort algorithm"), Order(4)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void TestQuickSortSamples(int testSampleSize)
        {
            var testArraySamples = new QuickSortingArrays(url: __trainingQuickSortingUrl).Arrays;

            QuickSortData quickSortData = (QuickSortData)testArraySamples[testSampleSize];
            var arrayToSort = quickSortData.arrayToSort;
            var expectedResponse = quickSortData.expectedResponse;

            List<Func<int[], int, int, int>> callbacksList = new List<Func<int[], int, int, int>>() {
                QuickSort.SortByLast, QuickSort.SortByFirst, QuickSort.SortByMedian
            };

            for (int callbackIdx = 0; callbackIdx < expectedResponse.Length; callbackIdx++)
            {
                var quickSortCallbackResult = callbacksList[callbackIdx](
                    (int[])arrayToSort.Clone(), 0, arrayToSort.Length - 1
                );
                Assert.AreEqual(quickSortCallbackResult, expectedResponse[callbackIdx]);
            }

            QuickSort.ResetCounters();
        }

        [Test(Author = "Me", Description = "Test for quick sort algorithm"), Order(5)]
        [TestCase(10000, new int[] { 150262, 159864, 130957 })]
        public void TestQuickSort(int testSampleSize, int[] expectedResponse)
        {
            var testArraySamples = new QuickSortingArrays(url: __quickSortUrl, expectedResponse: expectedResponse).Arrays;

            QuickSortData quickSortData = (QuickSortData)testArraySamples[testSampleSize];
            List<Func<int[], int, int, int>> callbacksList = new List<Func<int[], int, int, int>>() {
                QuickSort.SortByLast, QuickSort.SortByFirst, QuickSort.SortByMedian
            };

            for (int callbackIdx = 0; callbackIdx < quickSortData.expectedResponse.Length; callbackIdx++)
            {
                var quickSortCallbackResult = callbacksList[callbackIdx](
                    (int[])quickSortData.arrayToSort.Clone(), 0, quickSortData.arrayToSort.Length - 1
                );
                Assert.AreEqual(quickSortCallbackResult, expectedResponse[callbackIdx]);
            }

            QuickSort.ResetCounters();
        }

        [Test(Author = "Me", Description = "Test for radix sort algorithm"), Order(6)]
        public void TestRadixSortSimple()
        {
            var lettersRate = SQLiteClient.GetLettersRate();

            string[] baseStrings, sortedStrings;
            SQLiteClient.GetStringsCollections().UnpackTo(out baseStrings, out sortedStrings);

            var password = SQLiteClient.GetResultPassword();

            RadixSort.SortStrings(sample: baseStrings);
            Assert.AreEqual(baseStrings, sortedStrings);
            Assert.AreEqual(RadixSort.GeneratePassword(baseStrings), password);
        }

        [Test(Author = "Me", Description = "Test for radix sort algorithms"), Order(7)]
        [TestCase("aaolzzr")]
        public void TestRadixSortBigSample(string password)
        {
            string[] sample = WebClient.DownloadSamples(source: __radixSortUrl)
                                       .Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                                       .ToArray();

            RadixSort.SortStrings(sample);
            Assert.AreEqual(RadixSort.GeneratePassword(sample), password);
        }
    }
}
