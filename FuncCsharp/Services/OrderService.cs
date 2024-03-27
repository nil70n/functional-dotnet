using System.Collections.Immutable;
using FuncCsharp.Models;
using LanguageExt;

namespace FuncCsharp.Services;

public static class OrderService
{
    private static double SubTotal(OrderItem orderItem) => orderItem.Item.Price * orderItem.Quantity;

    private static double ApplyTaxToItem(OrderItem item, double rate) => item.Item.Taxable
        ? SubTotal(item) * rate + SubTotal(item)
        : SubTotal(item);

    private static double GetTaxRate(Order order, IEnumerable<TaxRate> taxRates) =>
        taxRates.FirstOrDefault(x => x.State == order.ShippingState)?.Rate ?? 1;

    private static Func<OrderItem, Discount, OrderItem> ApplyDiscount => (orderItem, discount) =>
        orderItem with
        {
            Item = orderItem.Item with
            {
                Price = orderItem.Item.Price - (orderItem.Item.Price * discount.Percentage / 100)
            }
        };

    private static Order ApplyDiscounts(Order order, List<Discount> discounts)
    {
        return discounts.Count == 0
            ? order
            : ApplyDiscounts(order with
            {
                Items = order.Items.Select(x => ApplyDiscount(x, discounts.First())).ToImmutableList()
            }, discounts.Skip(1).ToList());
    }

    private static Option<ShippingDetails> CallShipDetails(Order order) =>
        MemoExtensions.Memo(() => ShippingService.GetDetails(order)).Invoke();

    public static Order UpdateOrderShippingDetails(Order order)
    {
        return CallShipDetails(order).Match(
            Some: x => order with
            {
                ShippingState = x.DestinationState,
                EstimatedArrival = x.EstimatedArrival,
                ShippingCost = x.ShipCost
            },
            None: () => order);
    }

    /// <summary>
    /// Imperative Shell
    /// </summary>
    public static double GetTotal(Order order,
                                  IEnumerable<TaxRate> taxRates,
                                  IEnumerable<Discount> discounts)
    {
        var rate = GetTaxRate(order, taxRates);
        var discountedOrder = ApplyDiscounts(order, discounts.ToList());
        var grandTotal = discountedOrder.Items.Sum(x => ApplyTaxToItem(x, rate));

        return grandTotal;
    }

}
