//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using Razorpay.Api;
//using webapplication.Models;
//using webapplication.Repository;
//using System.Collections.Generic;
//using System.Linq;

//namespace webapplication.Controllers
//{
//    public class UserController : Controller
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly RazorpaySettings _razorpaySettings;

//        public UserController(IUserRepository userRepository, IOptions<RazorpaySettings> razorpaySettings)
//        {
//            _userRepository = userRepository;
//            _razorpaySettings = razorpaySettings.Value;
//        }

//        [HttpGet]
//        public IActionResult UserDashboard()
//        {
//            var courses = _userRepository.GetAllCourses();
//            return View(courses);
//        }

//        [HttpPost]
//        public IActionResult AddToCart(int courseId)
//        {
//            int userId = GetLoggedInUserId();
//            _userRepository.AddToCart(userId, courseId);
//            return RedirectToAction("UserDashboard");
//        }

//        [HttpGet]
//        public IActionResult Cart()
//        {
//            int userId = GetLoggedInUserId();
//            var cartItems = _userRepository.GetCartItems(userId);

//            decimal totalPrice = cartItems.Sum(ci => ci.Course.Price * ci.Quantity);

//            ViewBag.TotalPrice = totalPrice;
//            return View(cartItems);
//        }

//        [HttpPost]
//        public IActionResult DeleteFromCart(int courseId)
//        {
//            int userId = GetLoggedInUserId();
//            _userRepository.RemoveFromCart(userId, courseId);
//            return RedirectToAction("Cart");
//        }

//        [HttpGet]
//        public IActionResult MyCourses()
//        {
//            int userId = GetLoggedInUserId();
//            var myCourses = _userRepository.GetUserCourses(userId);
//            return View(myCourses);
//        }

//        [HttpGet]
//        public IActionResult WatchVideo(int courseId)
//        {
//            var topics = _userRepository.GetTopicsByCourseId(courseId);
//            var firstTopic = topics.FirstOrDefault();

//            if (firstTopic != null)
//            {
//                var comments = _userRepository.GetCommentsByTopicId(firstTopic.Id);
//                ViewBag.Comments = comments;
//            }

//            return View(topics);
//        }
//        [HttpPost]
//        public async Task<IActionResult> AddComment(int topicId, string content)
//        {
//            int userId = GetLoggedInUserId(); // Replace with your actual method to get logged-in user ID

//            var comment = new Comment
//            {
//                TopicId = topicId,
//                UserId = userId, // Convert userId to string if necessary
//                Content = content,
//                CreatedAt = DateTime.Now
//            };

//            _userRepository.AddComment(comment); // Assuming AddComment saves the comment to DB

//            int courseId = await _userRepository.GetCourseIdByTopicIdAsync(topicId); // Assuming async method
//            return RedirectToAction("WatchVideo", new { courseId });
//        }


//        [HttpPost]
//        public IActionResult MakePayment()
//        {
//            int userId = GetLoggedInUserId();
//            var cartItems = _userRepository.GetCartItems(userId);

//            // Set a smaller amount for testing
//            decimal testAmount = 5000; // ₹50

//            // Round the test amount to the nearest ₹100
//            decimal roundedPrice = Math.Ceiling(testAmount / 100) * 100;

//            var client = new RazorpayClient(_razorpaySettings.Key, _razorpaySettings.Secret);
//            var options = new Dictionary<string, object>
//        {
//            { "amount", roundedPrice * 100 }, // Amount in paise
//            { "currency", "INR" },
//            { "receipt", "test_receipt" },
//            { "payment_capture", 1 }
//        };

//            Order order = client.Order.Create(options);
//            string orderId = order["id"].ToString();

//            ViewBag.OrderId = orderId;
//            ViewBag.RazorpayKey = _razorpaySettings.Key;
//            ViewBag.Amount = roundedPrice * 100;

//            return View("Payment");
//        }


//        [HttpPost]
//        public IActionResult PaymentSuccess(string razorpayPaymentId, string razorpayOrderId, string razorpaySignature)
//        {
//            int userId = GetLoggedInUserId();
//            var cartItems = _userRepository.GetCartItems(userId);

//            string invoice = GenerateInvoice(cartItems);

//            foreach (var item in cartItems)
//            {
//                _userRepository.RemoveFromCart(userId, item.CourseId);
//                _userRepository.AddCourseToUser(userId, item.CourseId);
//            }

//            decimal totalPrice = cartItems.Sum(ci => ci.Course.Price * ci.Quantity);

//            // Add ViewBag properties for the Invoice view
//            ViewBag.OrderId = razorpayOrderId;
//            ViewBag.SubTotal = $"₹{totalPrice:0.00}";
//            ViewBag.Discount = "₹0.00"; // Add discount calculation if applicable
//            ViewBag.ShippingCharge = "₹0.00"; // Add shipping charge calculation if applicable
//            ViewBag.Tax = "₹0.00"; // Add tax calculation if applicable
//            ViewBag.Total = $"₹{totalPrice:0.00}";

//            ViewBag.Invoice = invoice;
//            return View("Invoice", cartItems); // Pass cartItems to the view
//        }

//        private string GenerateInvoice(IEnumerable<CartItem> cartItems)
//        {
//            string invoice = "Invoice\n\n";
//            foreach (var item in cartItems)
//            {
//                invoice += $"Course: {item.Course.Name}, Price: {item.Course.Price}, Quantity: {item.Quantity}\n";
//            }

//            decimal totalPrice = cartItems.Sum(ci => ci.Course.Price * ci.Quantity);
//            invoice += $"\nTotal: {totalPrice}";

//            return invoice;
//        }
//        public IActionResult ViewQuizzes(int topicId)
//        {
//            ViewBag.TopicId = topicId;
//            var quizzes = _userRepository.GetQuizzesByTopic(topicId);
//            return View(quizzes);
//        }


//        public IActionResult ViewAssignments(int topicId)
//        {
//            var assignments = _userRepository.GetAssignmentsByTopic(topicId);
//            return View(assignments);
//        }





//        [HttpPost]
//        public IActionResult SubmitQuiz(int topicId, Dictionary<int, string> userAnswers)
//        {
//            int userId = GetLoggedInUserId();
//            var quizzes = _userRepository.GetQuizzesByTopic(topicId);
//            int correctAnswersCount = 0;

//            foreach (var quiz in quizzes)
//            {
//                if (userAnswers.ContainsKey(quiz.Id) && userAnswers[quiz.Id] == quiz.CorrectOption)
//                {
//                    correctAnswersCount++;
//                }
//            }

//            var result = new UserQuizResult
//            {
//                UserId = userId,
//                TopicId = topicId,
//                Score = correctAnswersCount,
//                TotalQuestions = quizzes.Count(), // Corrected this line to call Count()
//                Date = DateTime.Now
//            };

//            _userRepository.AddUserQuizResult(result);

//            ViewBag.Score = correctAnswersCount;
//            ViewBag.TotalQuestions = quizzes.Count(); // Corrected this line to call Count()
//            ViewBag.CorrectOptions = quizzes.ToDictionary(q => q.Id, q => q.CorrectOption);

//            return View("QuizResult", quizzes);
//        }



//        private int GetLoggedInUserId()
//        {
//            // Replace with actual logic to get the logged-in user's ID
//            return 1;
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Razorpay.Api;
using webapplication.Models;
using webapplication.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly RazorpaySettings _razorpaySettings;

        public UserController(IUserRepository userRepository, IOptions<RazorpaySettings> razorpaySettings)
        {
            _userRepository = userRepository;
            _razorpaySettings = razorpaySettings.Value;
        }

        [HttpGet]
        public IActionResult UserDashboard()
        {
            var courses = _userRepository.GetAllCourses();
            return View(courses);
        }

        [HttpPost]
        public IActionResult AddToCart(int courseId)
        {
            int userId = GetLoggedInUserId();
            _userRepository.AddToCart(userId, courseId);
            return RedirectToAction("UserDashboard");
        }

        [HttpGet]
        public IActionResult Cart()
        {
            int userId = GetLoggedInUserId();
            var cartItems = _userRepository.GetCartItems(userId);

            decimal totalPrice = cartItems.Sum(ci => ci.Course.Price * ci.Quantity);

            ViewBag.TotalPrice = totalPrice;
            return View(cartItems);
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int courseId)
        {
            int userId = GetLoggedInUserId();
            _userRepository.RemoveFromCart(userId, courseId);
            return RedirectToAction("Cart");
        }

        [HttpGet]
        public IActionResult MyCourses()
        {
            int userId = GetLoggedInUserId();
            var myCourses = _userRepository.GetUserCourses(userId);
            return View(myCourses);
        }

        [HttpGet]
        public IActionResult WatchVideo(int courseId)
        {
            var topics = _userRepository.GetTopicsByCourseId(courseId);
            var firstTopic = topics.FirstOrDefault();

            if (firstTopic != null)
            {
                var comments = _userRepository.GetCommentsByTopicId(firstTopic.Id);
                ViewBag.Comments = comments;
            }

            return View(topics);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int topicId, string content)
        {
            int userId = GetLoggedInUserId(); // Replace with your actual method to get logged-in user ID

            var comment = new Comment
            {
                TopicId = topicId,
                UserId = userId, // Convert userId to string if necessary
                Content = content,
                CreatedAt = DateTime.Now
            };

            _userRepository.AddComment(comment); // Assuming AddComment saves the comment to DB

            int courseId = await _userRepository.GetCourseIdByTopicIdAsync(topicId); // Assuming async method
            return RedirectToAction("WatchVideo", new { courseId });
        }

        [HttpPost]
        public IActionResult MakePayment()
        {
            int userId = GetLoggedInUserId();
            var cartItems = _userRepository.GetCartItems(userId);

            // Set a smaller amount for testing
            decimal testAmount = 5000; // ₹50

            // Round the test amount to the nearest ₹100
            decimal roundedPrice = Math.Ceiling(testAmount / 100) * 100;

            var client = new RazorpayClient(_razorpaySettings.Key, _razorpaySettings.Secret);
            var options = new Dictionary<string, object>
            {
                { "amount", roundedPrice * 100 }, // Amount in paise
                { "currency", "INR" },
                { "receipt", "test_receipt" },
                { "payment_capture", 1 }
            };

            Order order = client.Order.Create(options);
            string orderId = order["id"].ToString();

            ViewBag.OrderId = orderId;
            ViewBag.RazorpayKey = _razorpaySettings.Key;
            ViewBag.Amount = roundedPrice * 100;

            return View("Payment");
        }

        [HttpPost]
        public IActionResult PaymentSuccess(string razorpayPaymentId, string razorpayOrderId, string razorpaySignature)
        {
            int userId = GetLoggedInUserId();
            var cartItems = _userRepository.GetCartItems(userId);

            string invoice = GenerateInvoice(cartItems);

            foreach (var item in cartItems)
            {
                _userRepository.RemoveFromCart(userId, item.CourseId);
                _userRepository.AddCourseToUser(userId, item.CourseId);
            }

            decimal totalPrice = cartItems.Sum(ci => ci.Course.Price * ci.Quantity);

            // Add ViewBag properties for the Invoice view
            ViewBag.OrderId = razorpayOrderId;
            ViewBag.SubTotal = $"₹{totalPrice:0.00}";
            ViewBag.Discount = "₹0.00"; // Add discount calculation if applicable
            ViewBag.ShippingCharge = "₹0.00"; // Add shipping charge calculation if applicable
            ViewBag.Tax = "₹0.00"; // Add tax calculation if applicable
            ViewBag.Total = $"₹{totalPrice:0.00}";

            ViewBag.Invoice = invoice;
            return View("Invoice", cartItems); // Pass cartItems to the view
        }

        private string GenerateInvoice(IEnumerable<CartItem> cartItems)
        {
            string invoice = "Invoice\n\n";
            foreach (var item in cartItems)
            {
                invoice += $"Course: {item.Course.Name}, Price: {item.Course.Price}, Quantity: {item.Quantity}\n";
            }

            decimal totalPrice = cartItems.Sum(ci => ci.Course.Price * ci.Quantity);
            invoice += $"\nTotal: {totalPrice}";

            return invoice;
        }

        public IActionResult ViewQuizzes(int topicId)
        {
            ViewBag.TopicId = topicId;
            var quizzes = _userRepository.GetQuizzesByTopic(topicId);
            return View(quizzes);
        }

        public IActionResult ViewAssignments(int topicId)
        {
            var assignments = _userRepository.GetAssignmentsByTopic(topicId);
            return View(assignments);
        }

        [HttpPost]
        public IActionResult SubmitQuiz(int topicId, Dictionary<int, string> userAnswers)
        {
            int userId = GetLoggedInUserId();
            var quizzes = _userRepository.GetQuizzesByTopic(topicId);
            int correctAnswersCount = 0;

            foreach (var quiz in quizzes)
            {
                if (userAnswers.ContainsKey(quiz.Id) && userAnswers[quiz.Id] == quiz.CorrectOption)
                {
                    correctAnswersCount++;
                }
            }

            var result = new UserQuizResult
            {
                UserId = userId,
                TopicId = topicId,
                Score = correctAnswersCount,
                TotalQuestions = quizzes.Count(), // Corrected this line to call Count()
                Date = DateTime.Now
            };

            _userRepository.AddUserQuizResult(result);

            ViewBag.Score = correctAnswersCount;
            ViewBag.TotalQuestions = quizzes.Count(); // Corrected this line to call Count()
            ViewBag.UserAnswers = userAnswers;
            ViewBag.CorrectOptions = quizzes.ToDictionary(q => q.Id, q => q.CorrectOption);

            return View("QuizResult", quizzes);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateCertificate(int courseId)
        {
            int userId = GetLoggedInUserId();
            var user = await _userRepository.GetUserByIdAsync(userId);
            var course = await _userRepository.GetCourseByIdAsync(courseId);

            if (user == null || course == null)
            {
                return NotFound();
            }

            var certificateViewModel = new CertificateViewModel
            {
                UserName = user.FullName,
                CourseName = course.Name,
                CompletionDate = DateTime.Now
            };

            return View("Certificate", certificateViewModel);
        }

        private int GetLoggedInUserId()
        {
            // Replace with actual logic to get the logged-in user's ID
            return 1;
        }


        public IActionResult Assignments()
        {
            var assignments = _userRepository.GetAllAssignments();
            return View(assignments);
        }

        public IActionResult DownloadAssignment(int id)
        {
            var assignment = _userRepository.GetAssignmentById(id);
            if (assignment == null)
            {
                return NotFound();
            }

            return File(assignment.FileContent, assignment.ContentType, assignment.FileName);
        }
    }
}
