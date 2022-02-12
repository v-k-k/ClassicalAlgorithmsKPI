using ClassicalAlgorithmsKPI.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ClassicalAlgorithmsKPI.DataGenerators
{
    public class GraphWithWeightsData
    {
        public List<int[][]> ExpectedResult = new List<int[][]>();
        public List<int[][]> Samples = new List<int[][]>();
        public List<int> VertexesAmount = new List<int>();
        public List<int> EdgesAmount = new List<int>();


        public GraphWithWeightsData(string source)
        {
            var data = WebClient.DownloadZippedSamples(source: source, semicolumnSeparator: true)
                                .Split(new string[] { ";", }, StringSplitOptions.RemoveEmptyEntries)
                                .ToArray();

            int midOfContent = data.Length / 2;
            
            for (int i = 0; i < midOfContent; i++)
            {
                var generalData = data[i].Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(item => item.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                                             .Select(s => int.Parse(s))
                                                             .ToArray())
                                         .First()
                                         .ToArray();
                VertexesAmount.Add(generalData[0]);
                EdgesAmount.Add(generalData[1]);

                Samples.Add(
                    data[i].Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(item => item.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)                                               
                                               .Select(s => int.Parse(s))
                                               .ToArray())
                           .Skip(1)
                           .ToArray()
                );
                ExpectedResult.Add(
                    data[midOfContent + i].Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(item => item.Split(new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                                              .Select(s => "--".Equals(s) ? int.MinValue : int.Parse(s))
                                                              .ToArray())
                                          .ToArray()
                );
            }
        }
    }
}
