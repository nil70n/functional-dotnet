using FuncCsharp.Models;

namespace FuncCsharp.Data;

public static class TestData
{
    public static List<TaxRate> TaxRates => new() {
            new(8, "CA"),
            new(7, "AZ"),
            new(5, "UT"),
    };

    public static Discount NewCustomer => new("New Customer", 10, true);
    public static Discount CaCustomer => new("CA Customer", 2, true);
    public static Discount WidgetFireSale => new("Widget Discount", 25, true);
    public static Discount NewYear => new("New Year", 15, false);

    public static Order CAOrder => new(
            [
                new (new Item("Widget", 12, true, new ItemOptions(5, "Large", 0)), 6),
                new (new Item("Dingbat", 54, false, new ItemOptions(9, "Special", 0)), 2)
            ],
            new Customer("John Doe", "CA", 0),
            "CA",
            153,
            DateTime.Now,
            0);

    public static Order AZOrder => new(
            [
                new (new Item("Widget", 12, true, null), 2),
                new (new Item("Foo", 4, true, null), 32),
                new (new Item("Bar", 10, true, null), 4),
                new (new Item("Dingbat", 54, false, new ItemOptions(9, "Special", 0)), 2)
            ],
            new Customer("John Doe", "AZ", 3),
            "AZ",
            434,
            DateTime.Now,
            0);

    public static Order UTOrder => new(
            [
                new (new Item("Widget", 12, true, new ItemOptions(5, "Special", 2)), 120),
                new (new Item("Thing", 2, false, new ItemOptions(3, "ScratchDent", -1)), 300)
            ],
            new Customer("John Doe", "CA", 25),
            "UT",
            34,
            DateTime.Now,
            0);

}
