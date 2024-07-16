using Microsoft.AspNetCore.Mvc.Rendering;

namespace webapplication.Models
{
    public class AddVideosViewModel
    {
        public int CourseId { get; set; }
        public string TopicName { get; set; }
        public string VideoUrl { get; set; }
        public List<SelectListItem> Courses { get; set; }
    }
}
