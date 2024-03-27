using FuncCsharp.Models;
using LanguageExt;

namespace FuncCsharp.Services;

public static class PromotionService
{
    public static Either<IOException, Discount> GetDiscount()
    {
        return new Random().Next(1, 10) % 2 == 0
            ? new IOException("Error calling the promotion service.")
            : new Discount("Recent Promo", 11, true);
    }
}
