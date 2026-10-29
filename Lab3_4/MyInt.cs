namespace Lab3_4;

public class MyInt
{
    public MyInt(int initialValue)
    {
        Value = initialValue;
    }

    public event EventHandler<MyInt, OverflowEventArgs>? Overflow;

    public int Value { get; private set; }

    public void Add(int value)
    {
        try
        {
            Value += value;
        }
        catch (OverflowException)
        {
            var eventArgs = new OverflowEventArgs() { CurrentValue = Value, Operation = OperationKind.Add, Operand = value };

            unchecked
            {
                Value += value;
            }
        
            Overflow?.Invoke(this, eventArgs);
        }
    }

    public void Subtract(int value)
    {
        try
        {
            Value -= value;
        }
        catch (OverflowException)
        {
            var eventArgs = new OverflowEventArgs() { CurrentValue = Value, Operation = OperationKind.Subtract, Operand = value };

            unchecked
            {
                Value -= value;
            }

            Overflow?.Invoke(this, eventArgs);
        }
    }

    public void Multiply(int value)
    {
        try
        {
            Value *= value;
        }
        catch (OverflowException)
        {
            var eventArgs = new OverflowEventArgs() { CurrentValue = Value, Operation = OperationKind.Multiply, Operand = value };

            unchecked
            {
                Value *= value;
            }

            Overflow?.Invoke(this, eventArgs);
        }
    }

    public void Divide(int value)
    {
        try
        {
            Value /= value;
        }
        catch (OverflowException)
        {
            var eventArgs = new OverflowEventArgs() { CurrentValue = Value, Operation = OperationKind.Divide, Operand = value };

            unchecked
            {
                Value /= value;
            }

            Overflow?.Invoke(this, eventArgs);
        }
    }
}
