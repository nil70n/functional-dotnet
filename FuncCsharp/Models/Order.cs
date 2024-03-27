using System.Collections.Immutable;

namespace FuncCsharp.Models;

public record Order(
        ImmutableList<OrderItem> Items,
        Customer? Customer,
        string ShippingState,
        int OrderNumber,
        DateTime OrderDate,
        double Total = 0.0,
        DateTime EstimatedArrival = default,
        double ShippingCost = 0.0);
