using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Final_Project.Controllers
{
    public class TriviaController : Controller
    {

        private readonly FinalProject_DbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public TriviaController(FinalProject_DbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions()
        {
            var client = _httpClientFactory.CreateClient();
            // Category 18 = Computers, Amount 10
            var response = await client.GetFromJsonAsync<TriviaResponse>("https://opentdb.com/api.php?amount=10&category=18&type=multiple");

            return Json(response?.Results);
        }

        [HttpPost]
        public async Task<IActionResult> SaveScore(int score)
        {
            if (User.Identity.IsAuthenticated)
            {
                var triviaScore = new TriviaScore
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Score = score,
                    DateTaken = DateTime.Now
                };
                _context.Add(triviaScore);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Not logged in" });
        }


    }
}

public class TriviaResponse { public List<TriviaResult> Results { get; set; } }
public class TriviaResult
{
    public string Question { get; set; }
    public string Correct_Answer { get; set; }
    public List<string> Incorrect_Answers { get; set; }
}