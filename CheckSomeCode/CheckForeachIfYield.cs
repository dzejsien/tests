using System;
using System.Collections.Generic;

namespace CheckSomeCode
{
    /*
     * It should call values only some many times as limit says. 
     */
    public class CheckForeachIfYield
    {
        public static IEnumerable<int> GetInt(int limit, Action<string> logMessage)
        {
            if (limit < 0)
                yield break;

            foreach (var value in GetEnumerable(limit, logMessage))
            {
                if (value >= limit)
                    yield break;

                logMessage($"first foreach and value: {value}");
                yield return value;
            }
        }

        private static IEnumerable<int> GetEnumerable(int limit, Action<string> logMessage)
        {
            int[] values = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

            foreach (var value in values)
            {
                logMessage(value.ToString());
                yield return value;
            }
        }
    }
}