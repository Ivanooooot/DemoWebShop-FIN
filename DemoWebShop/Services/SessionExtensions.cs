using DemoWebShop.Services.Cart;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

namespace DemoWebShop.Services;

public static class SessionExtensions
{
    public static void SetCartObjectAsJson(
        this ISession session,
        string key,
        object value
        )
    {
        session.SetString(
            key,
            JsonConvert.SerializeObject(value)
            );

    }
    public static List<CartItem> GetCartObjectFromJson(
        this ISession session,
        string key
        )
    {
        var value = session.GetString(key);
        return value == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(value);
    }
}
