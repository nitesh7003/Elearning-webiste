namespace webapplication.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public int UserId { get; set; } // Change from string to int
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual User User { get; set; }
    }
}
