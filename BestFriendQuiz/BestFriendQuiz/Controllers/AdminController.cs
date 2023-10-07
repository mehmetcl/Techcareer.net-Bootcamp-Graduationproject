using BestFriendQuiz.Context;
using BestFriendQuiz.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BestFriendQuiz.Controllers
{
   
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
       
        private readonly BFQContext _context;

        public AdminController(BFQContext context)
        {
            _context = context;
        }
        public ActionResult Index(bool showApproved = true)
        {
           
            var questions = _context.Questionss.ToList();

         
            var approvedQuestions = questions.Where(q => q.Approval).ToList();
            var unapprovedQuestions = questions.Where(q => !q.Approval).ToList();

          
            var selectedQuestions = showApproved ? approvedQuestions : unapprovedQuestions;

            return View(selectedQuestions);
        }

     
        public ActionResult Details(int id)
        {
            return View();
        }

      
        [HttpGet]
        [AllowAnonymous]

        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(Question que)
        {
            if (ModelState.IsValid)
            {
                int roleId = int.Parse(HttpContext.Request.Cookies["RoleId"]);
                que.Approval = roleId == 1?true:false;
                _context.Questionss.Add(que);
                await _context.SaveChangesAsync();
                return roleId == 1 ? RedirectToAction("Index", "Admin") : RedirectToAction("Survey", "Survey");
            }
            
            return View(que);
        }


       
        [HttpGet()]
        public ActionResult Edit(int id)
        {
            var question = _context.Questionss.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Question que)
        {
            if (ModelState.IsValid)
            {
                _context.Questionss.Update(que);
                _context.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }

            return View(que);
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            Question model = _context.Questionss.FindAsync(id).Result;
            _context.Questionss.Remove(model);
            _context.SaveChanges();
            return RedirectToAction("Index", "Admin");

        }
    }
}
