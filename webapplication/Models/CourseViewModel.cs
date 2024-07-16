namespace webapplication.Models
{
    public class CourseViewModel
    {
        public List<Course> Courses { get; set; } = new List<Course>(); // List of all courses

        public int SelectedCourseId { get; set; }  // The course selected by the user

        public string TopicName { get; set; } = string.Empty; // Initialize with empty string

        public string VideoUrl { get; set; } = string.Empty; // Initialize with empty string

        public string Name { get; set; } = string.Empty; // Initialize with empty string
        public IFormFile? Thumbnail { get; set; } // Nullable IFormFile
        public string Details { get; set; } = string.Empty; // Initialize with empty string
        public decimal Price { get; set; } // Non-nullable decimal
        public List<Topic> AddedTopics { get; set; } = new List<Topic>(); // Initialize the list

        public int TopicCount { get; set; }  // Count of topics for the selected course
        public List<Topic> Topics { get; set; } = new List<Topic>(); // Add this property for topics
    }
}
