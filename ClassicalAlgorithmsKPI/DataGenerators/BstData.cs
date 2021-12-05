using ClassicalAlgorithmsKPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassicalAlgorithmsKPI.DataGenerators
{
    public class BstData
    {
        public Dictionary<int, (int[], List<int>[])> Collection = new Dictionary<int, (int[], List<int>[])>();

        public BstData(string source)
        {
            var unzippedContent = WebClient.DownloadZippedSamples(source, semicolumnSeparator: true)
                                           .Split(new string[] { ";", }, StringSplitOptions.RemoveEmptyEntries)
                                           .ToArray();
            int midOfContent = unzippedContent.Length / 2;
            for (int i = 0; i < midOfContent; i++)
            {
                var sample = unzippedContent[i].Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(item => int.Parse(item))
                                               .ToArray();
                var result = unzippedContent[midOfContent + i].Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                                                              .Select(entry => entry.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                                                                    .Select(inner => int.Parse(inner))
                                                                                    .ToList())
                                                              .ToArray();
                Collection.Add(i, (sample, result));
            }
        }
    }
}
