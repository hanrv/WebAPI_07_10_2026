using Microsoft.AspNetCore.Mvc;

namespace WebAPI_07_10_2026.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducController : ControllerBase
    {
        private static readonly string[] Names =
        [
            "Papas fritas", "Gaseosa cola", "Hamburguesa clásica", "Chocolate con leche", "Galletas", "Jugo de naranja", "Perro caliente", "Palomitas de maíz", "Agua mineral", "Caramelos"
        ];

        private static readonly string[] Categories =
        [
            "Snacks", "Gaseosas", "Comida rápida", "Dulces", "Bebidas"
        ];

        private static readonly string[] Brands =
        [
            "Delisnack", "ColaMax", "Burger House", "Dulce Día", "Sabor Express"
        ];

        [HttpGet(Name = "GetProducts")]
        public IEnumerable<Product> Get()
        {
            return Enumerable.Range(1, 10).Select(id =>
            {
                var name = Names[Random.Shared.Next(Names.Length)];
                var brand = Brands[Random.Shared.Next(Brands.Length)];
                var price = Random.Shared.Next(50, 3001) / 100m;
                var quantity = Random.Shared.Next(0, 101);

                return new Product
                {
                    Id = id,
                    Name = $"{name} {Random.Shared.Next(100, 1000)}",
                    Brand = brand,
                    Category = Categories[Random.Shared.Next(Categories.Length)],
                    Price = price,
                    Currency = "EUR",
                    Quantity = quantity,
                    Description = $"{name} de la marca {brand}, listo para disfrutar.",
                    Sku = $"{brand[..2].ToUpperInvariant()}-{Random.Shared.Next(100000, 1000000)}",
                    Rating = Math.Round(Random.Shared.NextDouble() * 4 + 1, 1),
                    IsAvailable = quantity > 0,
                    ExpirationDate = DateOnly.FromDateTime(DateTime.Today.AddDays(Random.Shared.Next(3, 181)))
                };
            });
        }
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Currency { get; set; } = "EUR";

        public int Quantity { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Sku { get; set; } = string.Empty;

        public double Rating { get; set; }

        public bool IsAvailable { get; set; }

        public DateOnly ExpirationDate { get; set; }
    }
}
