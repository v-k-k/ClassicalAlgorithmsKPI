using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicalAlgorithmsKPI.Helpers
{
    public struct QuickSortData
    {
        public int[] arrayToSort;
        public int[] expectedResponse;

        public QuickSortData(int[] arrayToSort, int[] expectedResponse)
        {
            this.arrayToSort = arrayToSort;
            this.expectedResponse = expectedResponse;
        }
    }
}
