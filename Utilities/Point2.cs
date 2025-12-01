namespace AdventOfCode.Utilities;

/// <summary>
/// Represents a 2D point. The Y-Axis orientation is represented in order to help represent relative points.
/// </summary>
/// <param name="X">The x value for the point.</param>
/// <param name="Y">The y value for the point.</param>
/// <param name="YAxis">The y-axis orientation.</param>
public readonly record struct Point2(long X, long Y, YAxisDirection YAxis)
{
    public static readonly Point2 ZeroAtTopOrigin = new(0, 0, YAxisDirection.ZeroAtTop);
    public static readonly Point2 ZeroAtBottomOrigin = new(0, 0, YAxisDirection.ZeroAtBottom);

    public Point2 Left => Move(CompassDirection.West, 1);
    public Point2 Right => Move(CompassDirection.East, 1);
    public Point2 Above => Move(CompassDirection.North, 1);
    public Point2 Below => Move(CompassDirection.South, 1);

    public bool IsLeftOf(Point2 p) => X < p.X;
    public bool IsRightOf(Point2 p) => X > p.X;
    public bool IsAbove(Point2 p) => YAxis == YAxisDirection.ZeroAtBottom ? Y > p.Y : Y < p.Y;
    public bool IsBelow(Point2 p) => YAxis == YAxisDirection.ZeroAtBottom ? Y < p.Y : Y > p.Y;

    public Vec2 To(Point2 p) => new Vec2(p.X - X, p.Y - Y);
    public static Point2 operator +(Point2 p, Vec2 v) => new(p.X + v.X, p.Y + v.Y, p.YAxis);

    /// <summary>
    /// The orthogonal points are the four cardinal points adjacent to this point.
    /// </summary>
    public IEnumerable<Point2> OrthogonalPoints
    {
        get
        {
            yield return Above;
            yield return Right;
            yield return Below;
            yield return Left;
        }
    }

    /// <summary>
    ///  The adjacent points include all the points around this point, including the diagonal points.
    /// </summary>
    public IEnumerable<Point2> AdjacentPoints
    {
        get
        {
            yield return Above;
            yield return Above.Right;
            yield return Right;
            yield return Below.Right;
            yield return Below;
            yield return Below.Left;
            yield return Left;
            yield return Above.Left;
        }
    }

    /// <summary>
    ///  The adjacent points include all the points around this point, including the diagonal points.
    /// </summary>
    public IEnumerable<Point2> DiagonalAdjacentPoints
    {
        get
        {
            yield return Above.Right;
            yield return Below.Right;
            yield return Below.Left;
            yield return Above.Left;
        }
    }

    public long ManhattanDistance(Point2 p)
    {
        return Math.Abs(X - p.X) + Math.Abs(Y - p.Y);
    }

    public Point2 InDirection(CompassDirection direction)
    {
        return direction switch
        {
            CompassDirection.North => Above,
            CompassDirection.East => Right,
            CompassDirection.South => Below,
            CompassDirection.West => Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public Point2 Move(CompassDirection direction, long steps)
    {
        return direction switch
        {
            CompassDirection.North => this with { Y = YAxis == YAxisDirection.ZeroAtBottom ? Y + steps : Y - steps },
            CompassDirection.East => this with { X = X + steps },
            CompassDirection.South => this with { Y = YAxis == YAxisDirection.ZeroAtBottom ? Y - steps : Y + steps },
            CompassDirection.West => this with { X = X - steps },
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}