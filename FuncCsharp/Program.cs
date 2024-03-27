using FuncCsharp.Data;
using FuncCsharp.Models;
using FuncCsharp.Services;
using static FuncCsharp.Services.OrderService;

var taxRates = TestData.TaxRates;
var order = TestData.CAOrder;

var orders = new List<Order>()
{
    TestData.CAOrder,
    TestData.AZOrder,
    TestData.UTOrder
};

var discounts = new List<Discount>()
{
    TestData.NewCustomer,
    TestData.CaCustomer,
};

PromotionService.GetDiscount().Match(
    Left: ex => Console.WriteLine($"Error: {ex.Message}"),
    Right: discount => discounts.Add(discount)
);

var grandTotal = GetTotal(order, taxRates, discounts);

Console.WriteLine($"Grand total: {grandTotal:C}");
Console.WriteLine("Done processing.");
Console.ReadKey();
