using webapplication.Models;
using webapplication.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace webapplication.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User Login(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }

        public void AddToCart(int userId, int courseId)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(ci => ci.UserId == userId && ci.CourseId == courseId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    UserId = userId,
                    CourseId = courseId,
                    Quantity = 1
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += 1;
            }

            _context.SaveChanges();
        }

        public IEnumerable<CartItem> GetCartItems(int userId)
        {
            return _context.CartItems
                .Where(ci => ci.UserId == userId)
                .Include(ci => ci.Course)
                .ToList();
        }

        public void RemoveFromCart(int userId, int courseId)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(ci => ci.UserId == userId && ci.CourseId == courseId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                _context.SaveChanges();
            }
        }

        public void AddCourseToUser(int userId, int courseId)
        {
            var userCourse = new UserCourse
            {
                UserId = userId,
                CourseId = courseId
            };
            _context.UserCourses.Add(userCourse);
            _context.SaveChanges();
        }

        public IEnumerable<Course> GetUserCourses(int userId)
        {
            return _context.UserCourses
                .Where(uc => uc.UserId == userId)
                .Include(uc => uc.Course)
                .Select(uc => uc.Course)
                .ToList();
        }

        public IEnumerable<Topic> GetTopicsByCourseId(int courseId)
        {
            return _context.Topics
                .Where(t => t.CourseId == courseId)
                .ToList();
        }
        public IEnumerable<Comment> GetCommentsByTopicId(int topicId)
        {
            return _context.Comments
             .Where(c => c.TopicId == topicId)
             .Include(c => c.User)  // Include the related User
             .ToList();
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
        public int GetCourseIdByTopicId(int topicId)
        {
            var topic = _context.Topics.FirstOrDefault(t => t.Id == topicId);
            return topic != null ? topic.CourseId : 0;  // Assuming Topic has a CourseId property
        }

        public async Task<int> GetCourseIdByTopicIdAsync(int topicId)
        {
            // Implement logic to retrieve courseId by topicId from your DB
            // Example:
            var topic = await _context.Topics.FindAsync(topicId);
            if (topic != null)
            {
                return topic.CourseId;
            }
            return 0; // Handle accordingly if topic or course not found
        }
        //public IEnumerable<Quiz> GetQuizzesByTopic(int topicId)
        //{
        //    return _context.Quizzes.Where(q => q.TopicId == topicId).ToList();
        //}

        public IEnumerable<Assignment> GetAssignmentsByTopic(int topicId)
        {
            return _context.Assignments.Where(a => a.QuizId == topicId).ToList();
        }

        public List<Quiz> GetQuizzesByTopic(int topicId)
        {
            return _context.Quizzes.Where(q => q.TopicId == topicId).ToList();
        }

        public void AddUserQuizResult(UserQuizResult result)
        {
            _context.UserQuizResults.Add(result);
            _context.SaveChanges();
        }

        public List<UserQuizResult> GetUserQuizResults(int userId)
        {
            return _context.UserQuizResults.Where(r => r.UserId == userId).ToList();
        }

        //IEnumerable<Quiz> IUserRepository.GetQuizzesByTopic(int topicId)
        //{
        //    throw new NotImplementedException();
        //}

        public void SaveUserQuizResult(UserQuizResult result)
        {
            _context.UserQuizResults.Add(result);
            _context.SaveChanges();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses.FindAsync(courseId);
        }

        public IEnumerable<Assignment> GetAllAssignments()
        {
            return _context.Assignments.ToList();
        }

        public Assignment GetAssignmentById(int id)
        {
            return _context.Assignments.Find(id);
        }
    }
}
