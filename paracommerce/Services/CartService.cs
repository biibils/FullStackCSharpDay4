using paracommerce.Models;

namespace paracommerce.Services;

public class CartService : ICartService
{
    private readonly Cart _cart = new();

    public Cart GetCart() => _cart;

    public void AddToCart(Product product)
    {
        var item = _cart.Items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (item == null)
        {
            if (product.Stock <= 0)
                return;
            _cart.Items.Add(new CartItem { Product = product, Quantity = 1 });
        }
        else
        {
            Increase(product.Id);
        }
    }

    public void Increase(int productId)
    {
        var item = _cart.Items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null && item.Quantity < item.Product.Stock)
        {
            item.Quantity++;
        }
    }

    public void Decrease(int productId)
    {
        var item = _cart.Items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            item.Quantity--;
            if (item.Quantity <= 0)
            {
                _cart.Items.Remove(item);
            }
        }
    }

    public bool Checkout()
    {
        foreach (var item in _cart.Items)
        {
            if (item.Quantity > item.Product.Stock)
                return false;
        }

        foreach (var item in _cart.Items)
        {
            item.Product.Stock -= item.Quantity;
        }

        _cart.Items.Clear();
        return true;
    }
}
