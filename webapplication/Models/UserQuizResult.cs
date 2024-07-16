namespace webapplication.Models
{
    public class UserQuizResult
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int TopicId { get; set; }
        public int Score { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime Date { get; set; }
    }
}
