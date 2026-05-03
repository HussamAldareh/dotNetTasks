namespace Task2_MVC_.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<Products> Products { get; set; }
    }
}
