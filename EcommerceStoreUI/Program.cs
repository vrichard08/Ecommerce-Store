using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using V59Z7I_SOF_2023241.Models;

namespace EcommerceStoreUI
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:7183/api");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(responseBody);

                foreach (var product in products)
                {
                    Console.WriteLine($"Id: {product.Id}\nName: {product.Name}\nDescription: {product.Description}\nPrice: {product.Price}\nStockQuantity: {product.StockQuantity}\nCategoryId: {product.CategoryId}");
                    Console.WriteLine("------------------------------------------------------------------------------------------------------");
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }

    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; }
        public string CategoryId { get; set; }
    }
}