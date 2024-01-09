using System.ComponentModel.DataAnnotations;

namespace V59Z7I_SOF_2023241.Models
{
    public class Cart
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set;}

        public Cart()
        {
            Id = Guid.NewGuid().ToString();
            CartItems = new HashSet<CartItem>();
        }
    }
}
