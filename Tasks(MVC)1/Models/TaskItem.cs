namespace Tasks_MVC_1.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }

        public string ImportanceLevel { get; set; }

        public string UserId { get; set; }
        public Users? User { get; set; }
    }
}
