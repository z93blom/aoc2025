namespace AdventOfCode.Utilities;

public static class NumberExtensions
{
    extension(long value)
    {
        public long Min(long other)
        {
            return Math.Min(value, other);
        }

        public long Max(long other)
        {
            return Math.Max(value, other);
        }

        public long Abs()
        {
            return Math.Abs(value);
        }
    }

    extension(int value)
    {
        public int Min(int other)
        {
            return Math.Min(value, other);
        }

        public int Max(int other)
        {
            return Math.Max(value, other);
        }

        public int Abs()
        {
            return Math.Abs(value);
        }
    }
}