using System.ComponentModel.DataAnnotations;
using paracommerce.Models;

public class CartItem
{
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Subtotal => Product.Price * Quantity;
}
