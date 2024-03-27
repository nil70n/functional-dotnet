using FuncCsharp.Models;
using LanguageExt;

namespace FuncCsharp.Services;

public static class ShippingService
{
    public static Option<ShippingDetails> GetDetails(Order order)
    {
        return new Random().Next(1, 10) % 2 == 0
            ? Option<ShippingDetails>.None
            : Option<ShippingDetails>.Some(new ShippingDetails(12.20, order.ShippingState, DateTime.Now.AddDays(3)));
    }
}
