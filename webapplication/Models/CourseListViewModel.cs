namespace webapplication.Models
{
    public class CourseListViewModel
    {
        public List<Course> Courses { get; set; } = new List<Course>();
        public int SelectedCourseId { get; set; }
        public int TopicCount { get; set; }
        public List<Topic> Topics { get; set; } = new List<Topic>(); // Add this property
    }
}
