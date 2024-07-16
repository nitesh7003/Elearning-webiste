using webapplication.Models;

namespace webapplication.Repository
{
    public interface IAdminRepository
    {
        void AddCourse(Course course);
        Admin GetAdminByEmail(string email);
        List<Course> GetAllCourses();
        string GetCourseNameById(int courseId);
        void AddTopic(Topic topic);
        List<Topic> GetAllTopics();
        List<Topic> GetTopicsByCourseId(int courseId);  // Ensure this method is included
        int GetTopicCountByCourseId(int courseId);
        void AddQuiz(Quiz quiz);

        void AddAssignment(Assignment assignment);
        IEnumerable<Assignment> GetAllAssignments();
        Assignment GetAssignmentById(int id);
        // Add this line



    }
}
