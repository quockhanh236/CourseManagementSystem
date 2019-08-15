using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CourseManagementSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CourseManagementSystem.Controllers
{

    namespace CourseManagementSystem.Controllers
    {
        public class TestingController : Controller
        {
            private readonly ApplicationDbContext db = new ApplicationDbContext();

            public static List<string> answerList;
            public static int tk;


            // GET: Testing
            public ActionResult Question(int lectId)
            {

                if (!(db.Question.Count() > 0) && !(db.Test.Count() > 0))
                {
                    return View();
                }
                var t = db.Test.Where(l => l.Lecture.Id == lectId).ToList();
                ViewBag.TestId = t[0].Id;

                tk = t[0].Id;

                var questions = db.Question.Where(l => l.Test.Id == tk).Include(q => q.Answer);
                ViewBag.Questions = questions;
                var answers = ViewBag;
                //foreach (var item in questions)
                //{

                //    answers = (db.Answer.Where(a => a.QuestionId == item.QuestionId)).ToArray<Answer>().ToList<Answer>();
                //    int isRadio = answers.Count(answer => answer.IsTrue);
                //    ViewBag.IsCheckBox = (isRadio == 1);
                //}
                answerList = new List<string>();
                ViewBag.AnswerList = answerList;
                return View();
            }

            [HttpPost]
            [Authorize(Roles = "student")]
            public ActionResult Question()
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                string user = User.Identity.GetUserId();
                var count = Request.Form.Count;
                string[][] results = new string[count][];
                var tests = db.Test.Where(t => t.Id == tk).ToList();
                var t1 = tests[0];
                var lectures = db.Lecture.Where(l => l.Id == t1.Lecture.Id).ToList();
                for (int i = 0; i < count; i++)
                {
                    results[i] = Request.Form.GetValues(i);
                }
                int test = tk;
                var testAnswers =
                    db.Answer.Where(l => l.Question.Test.Id == test);
                var rightAnswers = testAnswers.Where(a => a.IsTrue).ToList();
                var questions = db.Question.Where(q => q.Test.Id == test).ToList();
                int qwestionPower = 0;
                foreach (var question in questions)
                {
                    switch (question.Importance)
                    {
                        case 1:
                            qwestionPower += 1;
                            break;
                        case 2:
                            qwestionPower += 2;
                            break;
                        case 3:
                            qwestionPower += 3;
                            break;
                    }
                }
                int right = 0;
                //if (testAnswers.Count(a => a.IsTrue) > 1)
                //{
                for (int i = 0; i < results.Length; i++)
                {
                    bool isTrueUserAnswer = true;
                    int questionId = questions[i].QuestionId;
                    List<Answer> rightAnswersToQuestion = db.Answer.Where(a => a.QuestionId == questionId && a.IsTrue).ToList();
                    if (results[i].Length != rightAnswersToQuestion.Count)
                        isTrueUserAnswer = false;
                    else
                        for (int j = 0; j < results[i].Length; j++)
                        {
                            if (results[i][j] != rightAnswersToQuestion[j].Id.ToString())
                                isTrueUserAnswer = false;
                        }
                    if (isTrueUserAnswer && questions[i].Importance == 1)
                    {
                        right++;
                    }
                    else if (isTrueUserAnswer && questions[i].Importance == 2)
                    {
                        right += 2;
                    }
                    else if (isTrueUserAnswer && questions[i].Importance == 3)
                    {
                        right += 3;
                    }
                    //if (results[i].Length > 1)
                    //{
                    //    for (int j = 0; j < results[i].Length; j++)
                    //    {
                    //        if (rightAnswers.Where(a => a.Id.ToString() == results[i][j]).FirstOrDefault().IsTrue == )
                    //    }
                    //}
                }

                //}
                //var rightAnswers = db.Answer.Where(a => a.IsTrue == true).Include(l => l.Question.Test.Id == test).ToList();



                //foreach (var marksSigment in rightAnswers)
                //{
                //    for (int i = 0; i < results.Length; i++)
                //        for (int j = 0; j < results[i].Length; j++)
                //            if (results[i].Length == 1)
                //            {
                //                if (marksSigment.Id.ToString() == results[i][j].ToString())
                //                {
                //                    right++;
                //                }
                //            }

                //}

                var l1 = lectures[0];
                var sub = db.Subscription.First(u => u.Subscriber.Id == user && u.Course.id == l1.Course.id);
                int mark = 0;
                if (right == qwestionPower)
                {
                    mark = 100;
                }
                else
                {
                    mark = 100 / qwestionPower * right;

                }
                var idCourse = db.Courses.First(c => c.id == l1.Course.id).id;
                
                db.Mark.Add(new Mark() { Value = mark, Student = userManager.FindById(user), Test = db.Test.First(t => t.Id == tk) });
                var isMarkExsist = db.Mark.Where(d => d.Student.Id == user && d.Test.Id == tk).ToList();
                if (isMarkExsist.Count == 1)
                {
                    return RedirectToAction("Details", "Mark", new { id = idCourse });
                }
                else
                {
                    db.SaveChanges();
                }
                return RedirectToAction("Details", "Mark", new { id = idCourse });
            }


            public ActionResult Retake(int lectId)
            {
                if (!(db.Question.Count() > 0) && !(db.Test.Count() > 0))
                {
                    return View();
                }
                var t = db.Test.Where(l => l.Lecture.Id == lectId).ToList();
                ViewBag.TestId = t[0].Id;
                tk = t[0].Id;
                var questions = db.Question.Where(l => l.Test.Id == tk).Include(q => q.Answer);
                ViewBag.Questions = questions;
                return View();
            }


            [HttpPost]
            [Authorize(Roles = "student")]
            public ActionResult Retake()
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                string user = User.Identity.GetUserId();
                var count = Request.Form.Count;
                string[][] results = new string[count][];
                for (int i = 0; i < count; i++)
                {
                    results[i] = Request.Form.GetValues(i);
                }
                int test = tk;
                var questions = db.Question.Where(q => q.Test.Id == test).ToList();
                int qwestionPower = 0;
                foreach (var question in questions)
                {
                    switch (question.Importance)
                    {
                        case 1:
                            qwestionPower += 1;
                            break;
                        case 2:
                            qwestionPower += 2;
                            break;
                        case 3:
                            qwestionPower += 3;
                            break;
                    }
                }
                int right = 0;
                for (int i = 0; i < results.Length; i++)
                {
                    bool isTrueUserAnswer = true;
                    int questionId = questions[i].QuestionId;
                    List<Answer> rightAnswersToQuestion = db.Answer.Where(a => a.QuestionId == questionId && a.IsTrue).ToList();
                    if (results[i].Length != rightAnswersToQuestion.Count)
                        isTrueUserAnswer = false;
                    else
                        for (int j = 0; j < results[i].Length; j++)
                        {
                            if (results[i][j] != rightAnswersToQuestion[j].Id.ToString())
                                isTrueUserAnswer = false;
                        }
                    if (isTrueUserAnswer && questions[i].Importance == 1)
                    {
                        right++;
                    }
                    else if (isTrueUserAnswer && questions[i].Importance == 2)
                    {
                        right += 2;
                    }
                    else if (isTrueUserAnswer && questions[i].Importance == 3)
                    {
                        right += 3;
                    }
                }
                int mark = 0;
                if (right == qwestionPower)
                {
                    mark = 100;
                }
                else
                {
                    mark = 100 / qwestionPower * right;

                }
                var userId = userManager.FindById(user);
                var idMark = db.Mark.Where(s => s.Student.Id == userId.Id && s.Test.Id == tk).ToList();
                var markId = idMark[0];
                Mark newMark = db.Mark.Find(markId.id);
                newMark.Value = mark;
                newMark.id = idMark[0].id;
                newMark.Test = idMark[0].Test;
                newMark.Student = idMark[0].Student;
                db.Entry(newMark).State = EntityState.Modified;
                db.SaveChanges();
                var tests = db.Test.Where(t => t.Id == tk).ToList();
                var t1 = tests[0];
                var lectures = db.Lecture.Where(l => l.Id == t1.Lecture.Id).ToList();
                var l1 = lectures[0];
                var idCourse = db.Courses.First(c => c.id == l1.Course.id).id;
                return RedirectToAction("Details", "Mark", new { id = idCourse });
            }


            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}