namespace Ars.Application.Exceptions;

public class SparePartOutOfStockException : Exception
{
    public string PartName { get; }
    public int RequestedQuantity { get; }
    public int AvailableQuantity { get; }
    public SparePartOutOfStockException(string partName, int requestedQuantity, int avaliableQuantity)
        : base($"The spare part '{partName}' is not available in the required quantity. Requested: {requestedQuantity}, in stock: {avaliableQuantity}.")
    {
        PartName = partName;
        RequestedQuantity = requestedQuantity;
        AvailableQuantity = avaliableQuantity;
    }
}
