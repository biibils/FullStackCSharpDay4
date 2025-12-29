using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace paracommerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [StringLength(500, ErrorMessage = "Deskripsi tidak boleh lebih dari 500 karakter.")]
        public required string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Harga tidak boleh 0 atau negatif.")]
        public required decimal Price { get; set; }

        [Required]
        public required int Stock { get; set; }
    }
}
