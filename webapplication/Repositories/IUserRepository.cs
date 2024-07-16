using System.Collections.Generic;
using System.Threading.Tasks; // Add this namespace for async Task
using webapplication.Models; // Make sure this namespace matches where Assignment and Quiz are defined

namespace webapplication.Repository
{
    public interface IUserRepository
    {
        void Register(User user);
        User Login(string email, string password);
        IEnumerable<Course> GetAllCourses();
        void AddToCart(int userId, int courseId);
        IEnumerable<CartItem> GetCartItems(int userId);
        void RemoveFromCart(int userId, int courseId);
        void AddCourseToUser(int userId, int courseId);
        IEnumerable<Course> GetUserCourses(int userId);
        IEnumerable<Topic> GetTopicsByCourseId(int courseId);
        IEnumerable<Comment> GetCommentsByTopicId(int topicId);
        void AddComment(Comment comment);
        int GetCourseIdByTopicId(int topicId);
        Task<int> GetCourseIdByTopicIdAsync(int topicId);
        //IEnumerable<Quiz> GetQuizzesByTopic(int topicId);
        IEnumerable<Assignment> GetAssignmentsByTopic(int topicId);

        List<Quiz> GetQuizzesByTopic(int topicId);
        void AddUserQuizResult(UserQuizResult result);
        List<UserQuizResult> GetUserQuizResults(int userId);

        Task<User> GetUserByIdAsync(int userId);
        Task<Course> GetCourseByIdAsync(int courseId);

        IEnumerable<Assignment> GetAllAssignments();
        Assignment GetAssignmentById(int id);
    }
}
