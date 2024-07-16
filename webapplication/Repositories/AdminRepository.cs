using webapplication.Models;
using webapplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace webapplication.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminRepository> _logger;
        private const string AdminEmail = "admin@example.com";  // Hardcoded admin email
        private const string AdminPassword = "AdminPassword123";  // Hardcoded admin password

        public AdminRepository(ApplicationDbContext context, ILogger<AdminRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddCourse(Course course)
        {
            try
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while saving the course.");
                throw new Exception("Error occurred while saving the course.", ex);
            }
        }

        public Admin GetAdminByEmail(string email)
        {
            // Check for hardcoded admin credentials
            if (email == AdminEmail)
            {
                return new Admin { Email = AdminEmail, Password = AdminPassword };
            }

            return _context.Admins.FirstOrDefault(a => a.Email == email);
        }

        public List<Course> GetAllCourses()
        {
            return _context.Courses.ToList();
        }

        public string GetCourseNameById(int courseId)
        {
            var course = _context.Courses.Find(courseId);
            return course?.Name ?? string.Empty;
        }

        public void AddTopic(Topic topic)
        {
            try
            {
                _context.Topics.Add(topic);
                _context.SaveChanges();
                _logger.LogInformation($"Topic {topic.TopicName} added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding topic: {topic.TopicName}");
                throw;
            }
        }

        public List<Topic> GetAllTopics()
        {
            return _context.Topics.ToList();
        }

        public List<Topic> GetTopicsByCourseId(int courseId)
        {
            return _context.Topics.Where(t => t.CourseId == courseId).ToList();
        }

        public int GetTopicCountByCourseId(int courseId)
        {
            return _context.Topics.Count(t => t.CourseId == courseId);
        }

        public void AddQuiz(Quiz quiz)
        {
            try
            {
                _context.Quizzes.Add(quiz);
                _context.SaveChanges();
                _logger.LogInformation($"Quiz for TopicId {quiz.TopicId} added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding quiz for TopicId {quiz.TopicId}");
                throw;
            }
        }

        public void AddAssignment(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            _context.SaveChanges();
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
