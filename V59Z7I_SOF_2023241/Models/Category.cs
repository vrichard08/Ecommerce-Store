using System.ComponentModel.DataAnnotations;

namespace V59Z7I_SOF_2023241.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
            Id = Guid.NewGuid().ToString();
            Products = new HashSet<Product>();
        }
    }
}
