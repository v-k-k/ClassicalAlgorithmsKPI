using System;
using System.Linq;

namespace ClassicalAlgorithmsKPI.Sorting
{
    public static class RadixSort
    {

        /// <summary>
        /// Finds the single digit in integer value by it's digit-position
        /// </summary>
        /// <param name="sourceNum"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private static int GetDigitByPosition(int sourceNum, int position)
        {
            int a = (int)(sourceNum % Math.Pow(10, position));
            int b = (int)(sourceNum % Math.Pow(10, position - 1));
            return (int)((a - b) / Math.Pow(10, position - 1));
        }

        /// <summary>
        /// Returs the multidimensional array with old and sorted indexes for the items in intArray 
        /// </summary>
        /// <param name="intArray"></param>
        /// <returns></returns>
        private static int[,] CountingSort(int[] intArray)
        {
            var debug = new int[intArray.Length];
            int max = int.MinValue;
            int[,] result = new int[intArray.Length, 3];
            for (int i = 0; i < intArray.Length; i++)
            {
                result[i, 0] = intArray[i];
                result[i, 1] = i;
                result[i, 2] = int.MinValue;
                if (intArray[i] > max)
                    max = intArray[i];
            }
            var countersArray = new int[max + 1];
            for (int i = 0; i < intArray.Length; i++)
                countersArray[intArray[i]]++;
            for (int j = 1; j <= max; j++)
                countersArray[j] += countersArray[j - 1];
            for (int i = intArray.Length - 1; i >= 0; i--)
            {
                var currentValue = intArray[i];
                for (int j = intArray.Length - 1; j >= 0; j--)
                {
                    if (result[j, 0] == currentValue && result[j, 2] == int.MinValue)
                    {
                        result[j, 2] = --countersArray[intArray[i]];
                        break;
                    }
                }
            }
            return result;
        }

        public static void SortInt(int[] sample, ref int[][] order)
        {
            var digits = Math.Floor(Math.Log10(sample[0]) + 1);
            for (int i = 1; i <= digits; i++)
            {
                var preparedArray = sample.Select(item => GetDigitByPosition(sourceNum: item, position: i)).ToArray();
                var clonedSample = (int[])sample.Clone();
                var sortinResult = CountingSort(intArray: preparedArray);
                for (int j = 0; j < preparedArray.Length; j++)
                    sample[sortinResult[j, 2]] = clonedSample[sortinResult[j, 1]];
                for (int j = 0; j < preparedArray.Length; j++)
                {
                    int idx = 0;
                    for (int k = 0; k < order[j].Length - 1; k++)
                    {
                        if (order[j][k + 1] == int.MinValue)
                        {
                            idx = k;
                            break;
                        }
                    }

                    for (int k = 0; k < order[j].Length; k++)
                    {
                        if (sortinResult[k, 1] == order[j][idx])
                        {
                            order[j][idx + 1] = sortinResult[k, 2];
                            break;
                        }
                    }
                }
            }
        }

        private static char getMaxOccurrenceCharacter(string[] sample)
        {
            string str = string.Join("", sample);
            int[] countArray = new int[256];
            int maxValue = 0;
            char resultChar = '\0';

            for (int i = 0; i < str.Length; i++)
            {
                countArray[str[i]]++;

                if (countArray[str[i]] >= maxValue)
                {
                    maxValue = countArray[str[i]];
                    resultChar = str[i];
                }
            }
            return resultChar;
        }

        public static string GeneratePassword(string[] sample)
        {
            char maxOccurrenceCharacter = getMaxOccurrenceCharacter(sample);
            return string.Format("{0}{1}{2}", sample[0], maxOccurrenceCharacter, sample[sample.Length - 1]);
        }

        public static void SortStrings(string[] sample)
        {
            for (int i = sample[0].Length - 1; i >= 0; i--)
            {
                var clonedSample = (string[])sample.Clone();
                int[] asciiSample = sample.Select(_char => (int)_char[i]).ToArray();
                var order = sample.Select(line => Enumerable.Repeat(int.MinValue, sample.Length).ToArray()).ToArray();                
                for (int j = 0; j < order.Length; j++)
                    order[j][0] = j;
                SortInt(sample: asciiSample, order: ref order);
                for (int j = 0; j < order.Length; j++)
                {
                    for  (int k = 0; k < order[j].Length; k++)
                    { 
                        if (order[j][k] == int.MinValue)
                            break;
                        sample[order[j][sample[0].Length]] = clonedSample[j];
                    }
                }
            }
        }
    }
}
