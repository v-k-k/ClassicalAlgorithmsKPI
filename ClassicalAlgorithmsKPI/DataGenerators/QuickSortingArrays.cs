using ClassicalAlgorithmsKPI.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClassicalAlgorithmsKPI.DataGenerators
{
    public class QuickSortingArrays
    {
        public IDictionary Arrays = new Dictionary<int, QuickSortData>();
        public QuickSortingArrays(string url)
        {
            var unzippedContent = WebClient.DownloadZippedSamples(source: url, semicolumnSeparator: true)
                                            .Split(new string[] { ";", }, StringSplitOptions.RemoveEmptyEntries);
            int midOfContent = unzippedContent.Length / 2;
            for (int i = 0; i < midOfContent; i++)
            {
                var splitted = unzippedContent[i].Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                                                    .Select(item => int.Parse(item))
                                                    .ToArray();
                var quickSortData = new QuickSortData(
                    arrayToSort: new ArraySegment<int>(splitted, 1, splitted.Length - 1).ToArray(),
                    expectedResponse: unzippedContent[midOfContent + i].Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                                                        .Select(item => int.Parse(item))
                                                                        .ToArray()
                );
                Arrays.Add(splitted[0], quickSortData);
            }
        }

        public QuickSortingArrays(string url, int[] expectedResponse)
        {
            var unzippedContent = WebClient.DownloadSamples(source: url)
                                               .Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(item => int.Parse(item))
                                               .ToArray();
            var quickSortData = new QuickSortData(
                arrayToSort: new ArraySegment<int>(unzippedContent, 1, unzippedContent.Length - 1).ToArray(),
                expectedResponse: expectedResponse
            );
            Arrays.Add(unzippedContent[0], quickSortData);
        }
    }
}
