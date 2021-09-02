using System;

namespace ClassicalAlgorithmsKPI.Sorting
{
    public static class MergeSort
    {

        private static int MergeAndCount(int[] array, int low, int mid, int high, int[] aux)
        {
            if (array[mid] <= array[mid + 1])
                return 0;

            int i = low;
            int j = mid + 1;
            int counter = 0;

            Array.Copy(array, low, aux, low, high - low + 1);

            for (int k = low; k <= high; k++)
            {
                if (i > mid)
                {
                    array[k] = aux[j++];
                    counter += mid + 1 - i;
                }
                else if (j > high) array[k] = aux[i++];
                else if (aux[j] < aux[i])
                {
                    array[k] = aux[j++];
                    counter += mid + 1 - i;
                }
                else array[k] = aux[i++];
            }
            return counter;
        }

        private static void Sort(int[] array, int low, int high, int[] aux)
        {
            if (high <= low)
                return;

            int mid = (high + low) / 2;
            Sort(array, low, mid, aux);
            Sort(array, mid + 1, high, aux);
            MergeAndCount(array, low, mid, high, aux);
        }

        public static void SortInt(ref int[] array)
        {
            int[] aux = new int[array.Length];
            Sort(array, 0, array.Length - 1, aux);
        }

        private static int SortAndCountInversions(int[] array, int low, int high, int[] aux)
        {
            if (high <= low)
                return 0;

            int mid = (high + low) / 2;
            int leftAmount = SortAndCountInversions(array, low, mid, aux);
            int rightAmount = SortAndCountInversions(array, mid + 1, high, aux);
            int mergedAmount = MergeAndCount(array, low, mid, high, aux);
            return leftAmount + rightAmount + mergedAmount;
        }

        public static int SortIntAndCountInversions(int[] array)
        {
            int[] aux = new int[array.Length];
            return SortAndCountInversions(array, 0, array.Length - 1, aux);
        }
    }
}
