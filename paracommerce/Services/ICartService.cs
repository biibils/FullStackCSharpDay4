using paracommerce.Models;

namespace paracommerce.Services;

public interface ICartService
{
    Cart GetCart();
    void AddToCart(Product product);
    void Increase(int productId);
    void Decrease(int productId);

    bool Checkout();
}
