using System;

public class OpenCellEventArgs : EventArgs
{
    public Position Position;

    public int Value;

    public OpenCellEventArgs(int x, int y, int value)
    {
        Position = new Position(x, y);
        Value = value;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is OpenCellEventArgs))
        {
            return false;
        }

        var anotherObj = (OpenCellEventArgs)obj;

        return anotherObj.Value == Value &&
            anotherObj.Position.X == Position.X &&
            anotherObj.Position.Y == Position.Y;
    }

    public override int GetHashCode()
    {
        return (Position.X + 1) * (Position.Y + 3) - Value;
    }
}

