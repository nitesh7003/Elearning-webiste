namespace webapplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }  // Add this property
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
