namespace Task2_MVC_.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImagePath { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
