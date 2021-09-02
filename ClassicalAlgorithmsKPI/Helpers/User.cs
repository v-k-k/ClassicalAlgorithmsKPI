using System;
using System.Linq;

namespace ClassicalAlgorithmsKPI.Helpers
{
    public class User
    {
        public int Number;
        public int[] Rate;

        public User(string rawUserRateString)
        {
            var userRate = rawUserRateString.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(Int32.Parse)
                                            .ToArray();
            Number = userRate[0];
            Rate = new int[userRate.Length - 1];
            Array.Copy(userRate, 1, Rate, 0, userRate.Length - 1);
        }
    }
}
