namespace FuncCsharp.Models;

public record ShippingDetails(
        double ShipCost,
        string DestinationState,
        DateTime EstimatedArrival);
