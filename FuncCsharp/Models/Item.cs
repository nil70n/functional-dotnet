namespace FuncCsharp.Models;

public record Item(
        string Name,
        int Price,
        bool Taxable,
        ItemOptions? Options);
