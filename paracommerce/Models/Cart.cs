using System.ComponentModel.DataAnnotations;

namespace paracommerce.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(i => i.Product.Price * i.Quantity);
    }
}
