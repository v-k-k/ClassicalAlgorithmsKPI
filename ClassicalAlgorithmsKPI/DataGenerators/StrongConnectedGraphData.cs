using ClassicalAlgorithmsKPI.Helpers;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClassicalAlgorithmsKPI.DataGenerators
{
    public class StrongConnectedGraphData
    {
        public int[][] ExpectedResult;
        public int[][][] Samples;

        public StrongConnectedGraphData(string source, bool useLastLines = true, int lines = 8)
        {
            var data = WebClient.DownloadZippedSamples(source: source, semicolumnSeparator: true)
                                .Split(new string[] { ";", }, StringSplitOptions.RemoveEmptyEntries)
                                .ToArray();
            if (useLastLines)
            {
                string[] tmpData = new string[lines];
                Array.Copy(data, data.Length - lines, tmpData, 0, lines);
                ExpectedResult = tmpData.Where(item => !item.Contains("\n"))
                                        .Select(item => item.Split(
                                            new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                                            .Select(s => int.Parse(s))
                                                            .OrderBy(amount => amount)
                                                            .ToArray()).ToArray();
                Samples = tmpData.Where(item => item.Contains("\n"))
                                 .Select(sample => sample.Split(
                                     new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                                                         .Select(item => item.Split(
                                         new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                                         .Select(s => int.Parse(s))
                                                         .ToArray()).ToArray()).ToArray();
            }
            else
            {
                ExpectedResult = data.Where(item => !item.Contains("\n"))
                                        .Select(item => item.Split(
                                            new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                                            .Select(s => int.Parse(s))
                                                            .OrderBy(amount => amount)
                                                            .ToArray()).ToArray();
                Samples = data.Where(item => item.Contains("\n"))
                              .Select(sample => sample.Split(
                                  new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(item => item.Split(
                                      new string[] { " ", }, StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(s => int.Parse(s))
                                                      .ToArray()).ToArray()).ToArray();
            }
        }
    }
}
