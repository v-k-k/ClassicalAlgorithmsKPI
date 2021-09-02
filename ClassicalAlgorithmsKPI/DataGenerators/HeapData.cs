using System;
using System.Linq;
using System.Collections.Generic;
using ClassicalAlgorithmsKPI.Helpers;
using System.Text.RegularExpressions;

namespace ClassicalAlgorithmsKPI.DataGenerators
{
    public class HeapData
    {
        private enum HeapDataIndexes
        {
            ExitOutput = 1, Heaps = 2, Input = 3, Output = 4
        }

        private readonly Regex keyPattern = new Regex(@"([A-Z])\w+");
        private readonly Regex valuePattern = new Regex(@"(?<=\: ).*");

        private string[] __medians;
        private string[] __heaps;
        private string[] __inputData;
        private string[] __outputData;

        public Dictionary<string, string>[][] Medians => __medians.Select(
            item => item.Split(new string[] { "\n\n", }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(intern => intern.Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                                                .ToDictionary(key => keyPattern.Matches(key)[0].Value,
                                                              value => valuePattern.Matches(value)[0].Value))
                        .ToArray()
        ).ToArray();
        public string[][][] Heaps => __heaps.Select(
            item => item.Split(new string[] { "\n\n", }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(intern => intern.Split('\n'))
                        .ToArray()
        ).ToArray();
        public int[][] InputData => __inputData.Select(
            item => item.Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(entry => int.Parse(entry))
                        .ToArray()
        ).ToArray();
        public int[][][] OutputData => __outputData.Select(
            item => item.Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(entry => entry.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                              .Select(inner => int.Parse(inner))
                                              .ToArray())
                        .ToArray()
        ).ToArray();

        public HeapData(string url)
        {
            var raw = WebClient.DownloadZippedSamples(source: url, semicolumnSeparator: true)
                               .Split(new string[] { ";", }, StringSplitOptions.RemoveEmptyEntries);
            var samplesCount = raw.Where(item => item.Contains("Medians")).Count();

            __medians = new string[samplesCount];
            __heaps = new string[samplesCount];
            __inputData = new string[samplesCount];
             __outputData = new string[samplesCount];

            for (int i = 0; i < raw.Length; i++)
            {
                var calculated = (i / samplesCount) + 1;
                switch (calculated)
                {
                    case (int)HeapDataIndexes.ExitOutput:
                        __medians[i % samplesCount] = raw[i];
                        break;
                    case (int)HeapDataIndexes.Heaps:
                        __heaps[i % samplesCount] = raw[i];
                        break;
                    case (int)HeapDataIndexes.Input:
                        __inputData[i % samplesCount] = raw[i];
                        break;
                    default:
                        __outputData[i % samplesCount] = raw[i];
                        break;
                }
            }
        }
    }
}
