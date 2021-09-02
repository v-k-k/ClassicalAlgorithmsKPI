using ClassicalAlgorithmsKPI.Helpers;
using System.Linq;
using System;

namespace ClassicalAlgorithmsKPI.DataStructures
{
    public class HashTable
    {
        private const int ALPHA = 3;
        private DoublyLinkedList<long>[] __itemsContainer;
        private int __tableSize = 0;

        public long[] __rawSamples;
        public long Max = long.MinValue;
        public long Min = long.MaxValue;

        public int TableSize {
            get { return __tableSize; }
            set { __tableSize = value % 2 == 0 ? ++value : value; }
        }

        private int GetHashByDivision(long entry)
        {
            return (int)Math.Abs(entry % TableSize);
        }

        public HashTable(string zippedValues)
        {
            __rawSamples = WebClient.DownloadZippedSamples(source: zippedValues)
                                    .Split(new string[] { "\n", }, StringSplitOptions.RemoveEmptyEntries)
                                    .Select(item => long.Parse(item))
                                    .ToArray();

            TableSize = __rawSamples.Length / ALPHA;
            __itemsContainer = Enumerable.Range(0, TableSize).Select(item => new DoublyLinkedList<long>()).ToArray();
            
            for (int i = 0; i < __rawSamples.Length; i++)
            {
                var entry = __rawSamples[i];
                if (entry > Max)
                    Max = entry;
                if (entry < Min)
                    Min = entry;
                var hashedIndex = GetHashByDivision(entry);
                __itemsContainer[hashedIndex].AddLast(entry);
            }
        }

        public bool HashSearch(long value)
        {
            bool result = false;
            var listContainer = __itemsContainer[GetHashByDivision(entry: value)];
            if (!listContainer.IsEmpty)
                result = listContainer.Contains(value);
            return result;
        }
    }
}
