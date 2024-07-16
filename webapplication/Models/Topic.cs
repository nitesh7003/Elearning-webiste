namespace webapplication.Models
{
    public class Topic
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string TopicName { get; set; }
        public string VideoUrl { get; set; }

    }
}
