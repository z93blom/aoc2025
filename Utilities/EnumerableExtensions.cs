using SuperLinq;

namespace AdventOfCode.Utilities;

public static class EnumerableExtensions
{
    extension<T>(IEnumerable<T> enumerable)
    {
        public IEnumerable<(T First, T Second)> Combinations()
        {
            var buffer = enumerable.Memoize();
            var numberOfItems = buffer.Count();
            for (var i = 0; i < numberOfItems - 1; i++)
            {
                var first = buffer.Skip(i).First();
                foreach (var second in buffer.Skip(i + 1))
                {
                    yield return (first, second);
                };
            }
        }

        public IEnumerable<(T First, T Second)> CircularWindows()
        {
            var buffer = enumerable.ToArray();
            for (var i = 0; i < buffer.Length; i++)
            {
                yield return (buffer[i], buffer[(i + buffer.Length + 1) % buffer.Length]);
            }
        }
    }
}