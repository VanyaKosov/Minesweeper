using System;

public class ToggleFlagEventArgs : EventArgs
{
    public Position Position;

    public FlagStatus FlagStatus;

    public int FlagsLeft;

    public ToggleFlagEventArgs(int x, int y, FlagStatus flagStatus, int flagsLeft)
    {
        Position = new Position(x, y);
        FlagStatus = flagStatus;
        FlagsLeft = flagsLeft;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is ToggleFlagEventArgs))
        {
            return false;
        }

        var anotherObj = (ToggleFlagEventArgs)obj;

        return anotherObj.Position.Equals(Position) &&
            anotherObj.FlagStatus == FlagStatus &&
            anotherObj.FlagsLeft == FlagsLeft;
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode() + (int)FlagStatus;
    }
}

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

        return anotherObj.Value == Value && anotherObj.Position.Equals(Position);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode() * (Value + 5);
    }
}

public class PositionEventArgs : EventArgs
{
    public Position Position;

    public PositionEventArgs(int x, int y)
    {
        Position.X = x;
        Position.Y = y;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is PositionEventArgs))
        {
            return false;
        }

        var anotherObj = (PositionEventArgs)obj;

        return Position.Equals(anotherObj.Position);
    }

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }
}