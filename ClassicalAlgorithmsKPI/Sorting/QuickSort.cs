using ClassicalAlgorithmsKPI.Base;
using System.Linq;

namespace ClassicalAlgorithmsKPI.Sorting
{
    public static class QuickSort
    {
        private static int __lastPivotPositionCounter = 0;
        private static int __firstPivotPositionCounter = 0;
        private static int __medianPivotPositionCounter = 0;

        private static int GetMedianElement(int[] array, int start, int end)
        {
            int swapValueIndex;
            if (array.Length == 2)
                swapValueIndex = start;
            else
            {
                int middle = (start + end) / 2;
                int[] helperArray = { array[start], array[middle], array[end] };
                int maxHelper = helperArray.Max();
                int minHelper = helperArray.Min();
                if (array[start] != maxHelper && array[start] != minHelper)
                    swapValueIndex = start;
                else if (array[middle] != maxHelper && array[middle] != minHelper)
                    swapValueIndex = middle;
                else
                    swapValueIndex = end;
            }
            Sorter.Swap(array, swapValueIndex, end);
            return array[end];
        }

        private static int Partition(int[] array, int start, int end, ref int counter, int median = int.MinValue)
        {
            int pivot = median == int.MinValue? array[end] : median;
            int position = start - 1;
            for (int i = start; i < end; i++)
            {
                if (array[i] <= pivot)
                {
                    position++;
                    Sorter.Swap(array, position, i);
                }
            }
            Sorter.Swap(array, ++position, end);
            counter += end - start;
            return position;
        }

        public static int SortByLast(int[] array, int p, int r)
        {
            if (p < r)
            {
                var q = Partition(array, p, r, ref __lastPivotPositionCounter);
                SortByLast(array, p, q - 1);
                SortByLast(array, q + 1, r);
            }
            return __lastPivotPositionCounter;
        }

        public static int SortByFirst(int[] array, int p, int r)
        {
            if (p < r)
            {
                Sorter.Swap(array, p, r);
                var q = Partition(array, p, r, ref __firstPivotPositionCounter);
                SortByFirst(array, p, q - 1);
                SortByFirst(array, q + 1, r);
            }
            return __firstPivotPositionCounter;
        }

        public static int SortByMedian(int[] array, int p, int r)
        {
            if (p < r)
            {
                int medianValue = GetMedianElement(array, start: p, end: r);
                var q = Partition(array, p, r, ref __medianPivotPositionCounter, median: medianValue);
                SortByMedian(array, p, q - 1);
                SortByMedian(array, q + 1, r);
            }
            return __medianPivotPositionCounter;
        }

        public static void ResetCounters()
        {
            __firstPivotPositionCounter = 0;
            __medianPivotPositionCounter = 0;
            __lastPivotPositionCounter = 0;
        }
    }
}
