namespace webapplication.Models
{
    public class QuizViewModel
    {
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Topic> Topics { get; set; } = new List<Topic>();
        public int SelectedCourseId { get; set; }
        public int SelectedTopicId { get; set; }
        public List<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public IFormFile Assignment { get; set; }
        public int NumberOfQuizzes { get; set; }
    }
}
