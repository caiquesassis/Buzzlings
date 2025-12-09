using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buzzlings.BusinessLogic.Utils
{
    public static class RandomUtils
    {
        private static readonly Random _random = new Random();

        public static T GetRandomEnumValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));

            return (T)values.GetValue(_random.Next(values.Length));
        }

        public static T GetRandomListElement<T>(List<T> list)
        {
            if (list is null || list.Count == 0)
            {
                throw new ArgumentException("List cannot be null or empty.", nameof(list));
            }

            return list[_random.Next(list.Count)];
        }

        public static int GetRandomRangeValue(int min, int max)
        {
            return _random.Next(min, max);
        }

        public static float GetRandomRangeValue(float min, float max)
        {
            return (float)(_random.NextDouble() * (min - max) + min);
        }
    }
}
