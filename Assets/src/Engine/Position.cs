
public struct Position
{
    public int X;

    public int Y;

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Position))
        {
            return false;
        }

        var anotherObject = (Position)obj;

        return X == anotherObject.X && Y == anotherObject.Y;
    }

    public override int GetHashCode()
    {
        return (X + 1) * (Y + 3);
    }
}

