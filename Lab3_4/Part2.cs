namespace Lab3_4;

public class Part2
{
    public void Run()
    {
        var myInt = new MyInt(int.MaxValue - 10);
        myInt.Overflow += MyInt_Overflow;
        myInt.Add(5);
        myInt.Add(10);
    }

    private void MyInt_Overflow(MyInt sender, OverflowEventArgs args)
    {
        Console.WriteLine($"Overflow:\nresult of {args.CurrentValue} {OperationKindToString(args.Operation)} {args.Operand} operation is out of int range.\nWrap-around took a place.\nCurrent value is {sender.Value}");
    }

    private string OperationKindToString(OperationKind operation)
    {
        return operation switch
        {
            OperationKind.Add => "+",
            OperationKind.Subtract => "-",
            OperationKind.Multiply => "*",
            OperationKind.Divide => "/",
            _ => throw new NotImplementedException()
        };
    }
}
