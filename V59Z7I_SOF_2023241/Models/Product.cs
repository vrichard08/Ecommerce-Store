using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace V59Z7I_SOF_2023241.Models
{
    public class Product
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(1, 1000000, ErrorMessage = "Price must be between 1 and 1000000.")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Stock Quantity is required.")]
        [Range(0, 9999, ErrorMessage = "Stock Quantity must be between 0 and 9999.")]
        public int StockQuantity { get; set; }
        public virtual Category Category { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public string CategoryId { get; set; }

        [StringLength(200)]
        [JsonIgnore]
        public string? ImageFileName { get; set; }

        [JsonIgnore]

        public string? ContentType { get; set; }

        [JsonIgnore]
        public byte[]? Data { get; set; }


        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
