using ClassicalAlgorithmsKPI.Helpers;
using ClassicalAlgorithmsKPI.Base;
using System.Collections.Generic;
using System;

namespace ClassicalAlgorithmsKPI.DataStructures
{
    public class Heap
    {
        private int[] __arrayHigh = { };
        private int[] __arrayLow = { };
        private int[] __high = { };
        private int[] __low = { };

        public int[] BuildMaxHeap(int[] array)
        {
            for (int i = array.Length / 2; i >= 0; i--)
                MaxHeapify(array, i);
            return array;
        }

        public int[] BuildMinHeap(int[] array)
        {
            for (int i = array.Length / 2; i >= 0; i--)
                MinHeapify(array, i);
            return array;
        }

        public int ExtractMax(ref int[] array)
        {
            int max = array[0];
            Sorter.Swap(array, 0, array.Length - 1);
            Array.Resize(ref array, array.Length - 1);
            MaxHeapify(array, 0);
            return max;
        }

        public int ExtractMin(ref int[] array)
        {
            int min = array[0];
            Sorter.Swap(array, 0, array.Length - 1);
            Array.Resize(ref array, array.Length - 1);
            MinHeapify(array, 0);
            return min;
        }

        public void MaxHeapify(int[] array, int start)
        {
            int left = 2 * start + 1;
            int right = 2 * start + 2;
            int largest = start;
            if (left < array.Length && array[left] > array[largest])
                largest = left;
            if (right < array.Length && array[right] > array[largest])
                largest = right;
            if (largest != start)
            {
                Sorter.Swap(array, start, largest);
                MaxHeapify(array, largest);
            }
        }
        
        public void MinHeapify(int[] array, int start)
        {
            int left = 2 * start + 1;
            int right = 2 * start + 2;
            int lowest = start;
            if (left < array.Length && array[left] < array[lowest])
                lowest = left;
            if (right < array.Length && array[right] < array[lowest])
                lowest = right;
            if (lowest != start)
            {
                Sorter.Swap(array, start, lowest);
                MinHeapify(array, lowest);
            }
        }
       
        public int[] FindMedium(int[] array, int start, out int[] medians, out int[] hLow, out int[] hHigh)
        {
            List<int> medium = new List<int>();
            array = array.Append(start);
            __arrayHigh = BuildMinHeap(__arrayHigh);
            __arrayLow = BuildMaxHeap(__arrayLow);
            if (array.Length == 1)
            {
                medium.Add(array[0]);
                __arrayHigh = __arrayHigh.Append(array[0]);
                __high = __high.Append(array[0]);
            }
            else
            {
                if (array.Length == 2)
                {
                    if (start > __arrayHigh[0])
                    {
                        __arrayLow = __arrayLow.Append(start);
                        int tmp = __arrayHigh[0];
                        __arrayHigh[0] = __arrayLow[0];
                        __arrayLow[0] = tmp;
                    }
                    else
                        __arrayLow = __arrayLow.Append(start);
                }
                else if (array.Length > 0 && start < __arrayLow[0])
                    __arrayLow = __arrayLow.Append(start);
                else
                    __arrayHigh = __arrayHigh.Append(start);
                if (__arrayHigh.Length - __arrayLow.Length > 1)
                    __arrayLow = __arrayLow.Append(ExtractMin(ref __arrayHigh));
                else if (__arrayLow.Length - __arrayHigh.Length > 1)
                    __arrayHigh = __arrayHigh.Append(ExtractMax(ref __arrayLow));
                __arrayHigh = BuildMinHeap(__arrayHigh);
                __arrayLow = BuildMaxHeap(__arrayLow);
                if (array.Length > 0)
                {
                    if (array.Length % 2 == 0)
                    {
                        medium.Add(__arrayLow[0]);
                        medium.Add(__arrayHigh[0]);
                    }
                    else if (__arrayHigh.Length > __arrayLow.Length)
                        medium.Add(__arrayHigh[0]);
                    else
                        medium.Add(__arrayLow[0]);
                }
                else
                    medium.Add(int.MinValue);
            }
            medians = medium.ToArray();
            hHigh = __arrayHigh;
            hLow = __arrayLow;
            return array;
        }
    }
}
