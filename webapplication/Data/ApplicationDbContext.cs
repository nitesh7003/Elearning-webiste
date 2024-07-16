using webapplication.Models;
using Microsoft.EntityFrameworkCore;

namespace webapplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<CartItem> CartItems { get; set; } // Add this line
        public DbSet<UserCourse> UserCourses { get; set; }

        public DbSet<Comment> Comments { get; set; }  // Add this line

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<UserQuizResult> UserQuizResults { get; set; }


    }
}
