using System.ComponentModel.DataAnnotations;

namespace V59Z7I_SOF_2023241.Models
{
    public class CartItem
    {
        [Key]
        public string Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public virtual Cart Cart { get; set; }
        public string CartId { get; set; }  
        public CartItem()
        {
            Id = Guid.NewGuid().ToString(); 

        }
    }
}
