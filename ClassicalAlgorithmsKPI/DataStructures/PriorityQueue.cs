using ClassicalAlgorithmsKPI.Helpers;
using System.Collections.Generic;

namespace ClassicalAlgorithmsKPI.DataStructures
{
    public class PriorityQueue
    {
        private List<int[]> __queue;

        public int Count => __queue.Count;

        public PriorityQueue(List<int[]> queue)
        {
            __queue = queue;
            for (int i = __queue.Count / 2; i >= 0; i--)
                MinHeapify(i);
        }

        private void MinHeapify(int position)
        {
            int left = 2 * position + 1;
            int right = 2 * position + 2;
            int lowest = position;
            if (left < __queue.Count && __queue[left][1] < __queue[lowest][1])
                lowest = left;
            if (right < __queue.Count && __queue[right][1] < __queue[lowest][1])
                lowest = right;
            if (lowest != position)
            {
                __queue = __queue.Swap(position, lowest) as List<int[]>;
                MinHeapify(lowest);
            }
        }

        public int ExtractMin()
        {
            int min = __queue[0][0];
            __queue = __queue.Swap(0, __queue.Count - 1) as List<int[]>;
            __queue.RemoveAt(__queue.Count - 1);
            MinHeapify(0);
            return min;
        }

        public void DecreaseKey(int value, int priority)
        {
            int position;
            for (int i = 0; i < __queue.Count; i++)
            {
                if (__queue[i][0] == value)
                {
                    __queue[i][1] = priority;
                    position = i;
                    while (position != 0 && __queue[(position - 1) / 2][1] > __queue[position][1])
                    {
                        __queue = __queue.Swap(position, (position - 1) / 2) as List<int[]>;
                        position = (position - 1) / 2;
                    }
                    break;
                }
            }
        }
    }
}
