namespace Lab3_4;

public class OverflowEventArgs : EventArgs
{
    public required int CurrentValue { get; init; }
    public required OperationKind Operation { get; init; }
    public required int Operand { get; init; }
}
