using BestFriendQuiz.Context;
using BestFriendQuiz.EntityLayer.Concrete;
using BestFriendQuiz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static BestFriendQuiz.Models.SurveyCreateViewModel;

namespace BestFriendQuiz.Controllers
{

    [Authorize(Roles = "User")]
    public class SurveyController : Controller
    {
        private readonly BFQContext _context;

        public SurveyController(BFQContext context)
        {
            _context = context;
        }


        [AllowAnonymous]
        public IActionResult Survey() //test sonuçlarının listelendiği Action
        {
            try
            {
                int roleId = int.Parse(HttpContext.Request.Cookies["RoleId"]);
            }
            catch (Exception)
            {
                return RedirectToAction("Create", "Guest");
            }

          
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            ViewData["UserId"] = userId;
            return View(_context.TestResults.Where(x => x.Test.UserId == userId).ToList());
        }

     
     
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            Test test = _context.Tests.FirstOrDefault(x => x.UserId == userId);
            if (test!=null)
            {   
                return RedirectToAction("Survey", "Survey");
            }
            var questions = _context.Questionss.Where(x => x.Approval == true).ToList();

            var surveyViewModel = new SurveyCreateViewModel
            {
                Questions = new List<SurveyCreateViewModel.QuestionWithOptionsViewModel>()
            };

            foreach (var question in questions)
            {
                var questionViewModel = new SurveyCreateViewModel.QuestionWithOptionsViewModel
                {
                    QuestionId = question.Id,
                    QuestionText = question.Quest,
                    SelectedQuestionId = null, // Başlangıçta hiçbir soru seçilmedi (null olarak ayarlandı)
                    Options = new List<SelectListItem>
            {
                new SelectListItem { Value = "A", Text = question.OptionA },
                new SelectListItem { Value = "B", Text = question.OptionB },
                new SelectListItem { Value = "C", Text = question.OptionC },
                new SelectListItem { Value = "D", Text = question.OptionD },
                new SelectListItem { Value = "E", Text = question.OptionE },
            },

                };

                surveyViewModel.Questions.Add(questionViewModel);
            }
            int roleId = int.Parse(HttpContext.Request.Cookies["RoleId"]);
            surveyViewModel.NumberOfQuestions = roleId == 2 ? 10 : 4;
            return View(surveyViewModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create(SurveyCreateViewModel surveyViewModel)
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            User user = _context.Users.FirstOrDefault(x => x.Id == userId);
            Test test = new Test();
            List<QuestionOfSurvey> questions = new List<QuestionOfSurvey>();
            foreach (var item in surveyViewModel.Questions)
            {
                QuestionOfSurvey questionOfSurvey = new QuestionOfSurvey();
                questionOfSurvey.Question = _context.Questionss.FirstOrDefault(x => x.Id == item.SelectedQuestionId);
                questionOfSurvey.RightOption = item.Option;
                questionOfSurvey.User = user;
                questionOfSurvey.UserId = userId;
                questions.Add(questionOfSurvey);
            }
            test.QuestionsOfSurvey = questions;
            Test newTest = new Test();
            newTest.User = user;
            newTest.UserId = userId;
            newTest.QuestionsOfSurvey = test.QuestionsOfSurvey;

            _context.Tests.Add(newTest);
            _context.SaveChanges();

            return RedirectToAction("Link", "Survey");

        }
        [AllowAnonymous]
        public IActionResult Link()
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            ViewData["Key"] = _context.Users.FirstOrDefault(x => x.Id == userId).Key;
            return View();
        }
        [HttpGet("Survey/[action]/{key}")]
        [AllowAnonymous]
        public IActionResult Solve(string key)
        {
            SolveViewModel solveViewModel = new SolveViewModel();
           
            User user=_context.Users.FirstOrDefault(x => x.Key.ToString() == key);
            if (user.RoleId==3 && user.Limit <= 0)
            {
                return RedirectToAction("Warning", "Guest");
            }
            Test test = _context.Tests.Include(t => t.QuestionsOfSurvey)
                 .ThenInclude(qs => qs.Question).FirstOrDefault(x => x.User.Key.ToString() == key);

            if (test != null)
            {
                foreach (var questionOfSurvey in test.QuestionsOfSurvey)
                {
                    questionOfSurvey.RightOption = null;
                }
            }
           
            solveViewModel.Test = test;
            return View(solveViewModel);
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Solve(SolveViewModel solve)
        {
            Test test=_context.Tests.FirstOrDefault(x=>x.Id == solve.Test.Id);
            User user = _context.Users.FirstOrDefault(x => x.Id == test.UserId);
            if (user.RoleId == 3)
            {
                user.Limit--;
                _context.Users.Update(user);
               
            }
            TestResult testResult = new TestResult();
            testResult.Name=solve.SolverName;
            testResult.Test = _context.Tests.Include(t => t.QuestionsOfSurvey).FirstOrDefault(x => x.Id == solve.Test.Id);
            testResult.Note = 0;
            for (int i = 0; i < testResult.Test.QuestionsOfSurvey.Count(); i++)
            {
                if (solve.Results[i].SelectedOption == testResult.Test.QuestionsOfSurvey[i].RightOption)
                {
                    testResult.Note++;
                }

            }
            testResult.TestId=solve.Test.Id;
            _context.TestResults.Add(testResult);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
