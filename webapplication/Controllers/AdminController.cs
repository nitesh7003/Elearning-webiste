using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using webapplication.Repository;
using webapplication.Models;
using System.IO;
using System.Linq;
using System;

namespace webapplication.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminRepository adminRepository, IWebHostEnvironment hostEnvironment, ILogger<AdminController> logger)
        {
            _adminRepository = adminRepository;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCourse(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.Thumbnail != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");
                    Directory.CreateDirectory(uploadsFolder);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Thumbnail.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Thumbnail.CopyTo(fileStream);
                    }
                }

                Course newCourse = new Course
                {
                    Name = model.Name,
                    Thumbnail = uniqueFileName,
                    Details = model.Details,
                    Price = model.Price
                };

                _adminRepository.AddCourse(newCourse);
                _logger.LogInformation($"Course {newCourse.Name} added successfully.");

                return RedirectToAction("AdminDashboard", "Home");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddTopic()
        {
            var viewModel = new CourseViewModel
            {
                Courses = _adminRepository.GetAllCourses(),
                AddedTopics = _adminRepository.GetAllTopics()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddTopic(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var topic = new Topic
                {
                    CourseId = model.SelectedCourseId,
                    CourseName = _adminRepository.GetCourseNameById(model.SelectedCourseId),
                    TopicName = model.TopicName,
                    VideoUrl = model.VideoUrl
                };

                _adminRepository.AddTopic(topic);

                // Update TopicCount
                model.TopicCount = _adminRepository.GetTopicCountByCourseId(model.SelectedCourseId);

                // Re-fetch courses and topics
                model.Courses = _adminRepository.GetAllCourses();
                model.AddedTopics = _adminRepository.GetAllTopics();

                return RedirectToAction("AddTopic");
            }

            // In case of invalid model state, re-fetch courses and topics
            model.Courses = _adminRepository.GetAllCourses();
            model.AddedTopics = _adminRepository.GetAllTopics();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddQuiz()
        {
            var viewModel = new QuizViewModel
            {
                Courses = _adminRepository.GetAllCourses()
            };
            return View(viewModel);
        }

        //[HttpPost]
        //public IActionResult AddQuiz(QuizViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var quiz in model.Quizzes)
        //        {
        //            quiz.TopicId = model.SelectedTopicId;
        //            _adminRepository.AddQuiz(quiz);
        //        }

        //        if (model.Assignment != null)
        //        {
        //            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Assignment.FileName);
        //            string filePath = Path.Combine(_hostEnvironment.WebRootPath, "assignments", uniqueFileName);
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                model.Assignment.CopyTo(fileStream);
        //            }
        //        }

        //        return RedirectToAction("AdminDashboard", "Home");
        //    }

        //    model.Courses = _adminRepository.GetAllCourses();
        //    return View(model);
        //}

        [HttpPost]
        public IActionResult AddQuiz(QuizViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var quiz in model.Quizzes)
                {
                    quiz.TopicId = model.SelectedTopicId;
                    _adminRepository.AddQuiz(quiz);

                    if (model.Assignment != null)
                    {
                        var assignment = new Assignment
                        {
                            FileName = model.Assignment.FileName,
                            ContentType = model.Assignment.ContentType,
                            QuizId = quiz.Id
                        };

                        using (var memoryStream = new MemoryStream())
                        {
                            model.Assignment.CopyTo(memoryStream);
                            assignment.FileContent = memoryStream.ToArray();
                        }

                        _adminRepository.AddAssignment(assignment);
                    }
                }

                return RedirectToAction("AdminDashboard", "Home");
            }

            model.Courses = _adminRepository.GetAllCourses();
            return View(model);
        }

        [HttpGet]
        public IActionResult GetTopics(int id)
        {
            var topics = _adminRepository.GetTopicsByCourseId(id);
            var result = topics.Select(topic => new { id = topic.Id, topicName = topic.TopicName });
            return Json(result);
        }
        [HttpGet]

        public IActionResult AdminDashboard()

        {
            return View();  

        }

        [HttpGet]
        public IActionResult CourseList()
        {
            var viewModel = new CourseListViewModel
            {
                Courses = _adminRepository.GetAllCourses()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CourseList(CourseListViewModel model)
        {
            model.Courses = _adminRepository.GetAllCourses();
            model.Topics = _adminRepository.GetTopicsByCourseId(model.SelectedCourseId);
            model.TopicCount = model.Topics.Count;
            return View(model);
        }



        public IActionResult Assignments()
        {
            var assignments = _adminRepository.GetAllAssignments();
            return View(assignments);
        }

        public IActionResult DownloadAssignment(int id)
        {
            var assignment = _adminRepository.GetAssignmentById(id);
            if (assignment == null)
            {
                return NotFound();
            }

            return File(assignment.FileContent, assignment.ContentType, assignment.FileName);
        }
    }
}
