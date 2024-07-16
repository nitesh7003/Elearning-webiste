namespace webapplication.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public int QuizId { get; set; }


    }
}
