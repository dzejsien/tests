using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckSomeCode
{
    /*
     * It should call values only some many times as limit says. 
     */
    public class CheckForeachIfYield : IChecker
    {
        public class Config
        {
            public readonly int Limit;

            public Config(int limit)
            {
                this.Limit = limit;
            }
        }

        public void Check(Action<string> logMessage, object data)
        {
            var config = data as Config ?? throw new ArgumentException(nameof(data));
            GetInt(config.Limit, logMessage).ToList();
        }

        private static IEnumerable<int> GetInt(int limit, Action<string> logMessage)
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
            int[] values = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (var value in values)
            {
                logMessage(value.ToString());
                yield return value;
            }
        }
    }
}