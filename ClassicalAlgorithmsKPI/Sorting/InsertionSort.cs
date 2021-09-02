

namespace ClassicalAlgorithmsKPI.Sorting
{
    public static class InsertionSort
    {
        public static void SortInt(ref int[] sample)
        {
            for (int i = 1; i < sample.Length; i++)
            {
                int key = sample[i];
                int j = i - 1;
                while (j >= 0 && sample[j] > key)
                {
                    sample[j + 1] = sample[j];
                    j = j - 1;
                }
                sample[j + 1] = key;
            }
        }
    }
}
