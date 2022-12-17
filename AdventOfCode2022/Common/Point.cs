namespace AdventOfCode2022.Common;

internal class Point
{
    public int X { get; set; }
        
    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static Point Empty => new(0, 0);

    public override string ToString()
    {
        return $"{this.X} {this.Y}";
    }

    public int GetManhattanDistanceTo(int x, int y)
    {
        return Math.Abs(this.X - x) + Math.Abs(this.Y - y);
    }

    public int GetManhattanDistanceTo(Point point)
    {
        return Math.Abs(this.X - point.X) + Math.Abs(this.Y - point.Y);
    }

    protected bool Equals(Point other)
    {
        return this.X == other.X && this.Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Point)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.X, this.Y);
    }
    
    public static bool operator ==(Point first, Point second)
    {
        return Equals(first, second);
    }
    
    public static bool operator !=(Point first, Point second)
    {
        return !Equals(first, second);
    }
}