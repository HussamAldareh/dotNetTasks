namespace Tasks_MVC_1.Models
{
    public class Department
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
