using Microsoft.AspNet.Identity;
using QuizK101.Models;
using QuizK101.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace QuizK101.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Quiz
        public ActionResult QuizInfo(int? id)
        {
            if (id == null) return HttpNotFound();
            HomeVM vm = new HomeVM();
            vm.Exam = db.Exams.FirstOrDefault(e => e.ID == id);
            if (vm.Exam == null) return HttpNotFound();
            return View(vm);
        }
        [HttpPost]
        public ActionResult QuizInfo(int? ExamId, int? q_start, int? q_end, int? q_ids)
        {
            if (ExamId == null) return HttpNotFound();
            var exam = db.Exams.Find(ExamId);
                HomeVM vm = new HomeVM();
                vm.Exam=exam;
            if (exam.Questions.Count > 0)
            {
                q_start = q_start.HasValue ? q_start.Value : 1;
                q_end = q_end.HasValue ? q_end.Value : 1;
                var userId = User.Identity.GetUserId();
                Quiz selectedQuiz = db.Quizs.FirstOrDefault(x => x.ExamID == ExamId &&
                x.UserID == userId && !x.QuizActive);

                if (selectedQuiz == null)
                {
                    selectedQuiz = CreateQuiz(ExamId.Value, q_start.Value, q_end.Value, q_ids);
                }
                else if (selectedQuiz.QuizEnd <= DateTime.Now)
                {
                    selectedQuiz = CreateQuiz(ExamId.Value, q_start.Value, q_end.Value, q_ids);
                }
                if (!q_ids.HasValue && q_ids > selectedQuiz.Exam.Questions.Count) return HttpNotFound();

                return RedirectToAction("StartExam", new { quizId = selectedQuiz.ID, examId = ExamId });
            }
            return View(vm);
        }

        public static HashSet<t> GetRandomElements<t>(IEnumerable<t> list, int elementsCount)
        {
            return list.OrderBy(x => Guid.NewGuid()).Take(elementsCount).ToHashSet();
        }
        private Quiz CreateQuiz(int exmId, int q_start, int q_end, int? q_ids)
        {
            q_start = q_start == 1 ? 0 : q_start;
            var quest = db.Questions.Select(x => x.ID).OrderBy(x => x).Skip(q_start).Take(q_end).ToList();
            HashSet<int> existingNumbers = new HashSet<int>();
            existingNumbers = GetRandomElements(quest, q_ids.Value);
            Quiz quiz = new Quiz()
            {
                ExamID = exmId,
                UserID = User.Identity.GetUserId(),
                QuizStart = DateTime.Now,
                //TimeSpan time = new TimeSpan(2000, 0, 0, 0);
                QuizEnd = DateTime.Now.AddHours(1)
            };

            quiz.QuizQuestions = new List<QuizQuestion>();
            quiz.QuizQuestions.AddRange(existingNumbers.Select(x => new QuizQuestion() { questionID = x, quizID = quiz.ID }));
            db.Quizs.Add(quiz);
            db.SaveChanges();
            return quiz;
        }

        public ActionResult QuizResult()
        {
            var userId = User.Identity.GetUserId();
            var myQuiz = db.Quizs.Where(x => x.UserID == userId && x.QuizActive).OrderByDescending(x => x.ID).ToList();
            var quezVm = new ResultQuizVM()
            {
                Quizzes = myQuiz
            };
            return View(quezVm);
        }
        public ActionResult QuizResultAnswer(int? id)
        {
            if (id == null) return HttpNotFound();
            ResultQuizVM quizVm = new ResultQuizVM()
            {
                QuizeSingle = db.Quizs.FirstOrDefault(x => x.ID == id)
            };
            return View(quizVm);
        }
        public ActionResult StartExam(int? QuizId, int? q_start, int? q_end)
        {
            if (QuizId == null) return HttpNotFound();
            QuestionViewModel vm = new QuestionViewModel();
            vm.Quiz = db.Quizs.FirstOrDefault(x => x.ID == QuizId);
            if (vm.Quiz == null) return HttpNotFound();
            vm.TotalQuestionCount = db.Questions.Where(x => x.ExamID == vm.Quiz.ExamID).Count();
            return View(vm);
        }
        public ActionResult EndQuiz(int? QuizId)
        {
            int score = 0;
            if (QuizId != null)
            {
                var activeQ = db.Quizs.First(x => x.ID == QuizId);
                var qz = db.QuizQuestions.Where(q => q.quizID == QuizId).ToList();
                if (qz == null) return HttpNotFound();
                ResultQuizVM vm = new ResultQuizVM();
                foreach (var quiz in qz)
                {
                    if (!quiz.Quiz.QuizActive)
                    {
                        if (quiz.myanswerID == quiz.Question.CorrectAnswerID)
                        {
                            score++;
                        }
                    }
                    quiz.Quiz.Score = score;
                    db.SaveChanges();
                }
                activeQ.QuizActive = true;
                activeQ.QuizEnd = DateTime.Now;
                activeQ.Score = score;
                vm.Score = score;
                db.SaveChanges();
                return View(vm);
            }
            return View();

        }
        [HttpPost]
        public ActionResult SubmitAnswer(int? QuizId, int? answerId, int? questionId)
        {
            if (answerId != null && QuizId != null && questionId != null)
            {
                var examQuest = db.QuizQuestions.Where(x => x.questionID == questionId && x.quizID == QuizId).First();
                examQuest.myanswerID = answerId;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = true });
        }
        public ActionResult QuestionSlide(int? QuizId, int? skip, int? answerId, int? questionId)
        {
            if (QuizId == null) return HttpNotFound();
            skip = skip.HasValue ? skip.Value > 0 ? skip.Value : 0 : 0;
            QuestionViewModel vm = new QuestionViewModel();
            var examQuest = db.QuizQuestions.Where(x => x.quizID == QuizId)
                .OrderBy(x => x.ID)
               .Skip(skip.Value).Take(1).FirstOrDefault();
            if (examQuest == null) return HttpNotFound();

            vm.Quiz = db.Quizs.FirstOrDefault(x => x.ID == QuizId);
            vm.CurrentQuestion += skip.Value;
            vm.TotalQuestionCount = db.Questions.Where(x => x.ExamID == examQuest.Quiz.ExamID).Count();
            vm.QuizQuestion = examQuest;

            return PartialView("QuestionSlide", vm);
        }
    }
}
// .Select(x => new QuestionDto()
//{
//    ID = x.ID,
//                    Name = x.Name,
//                    Answers = db.Answers
//                .Where(ans => ans.QuestionID == x.ID).Select(ans => new AnswerDto()
//                {
//                    ID = ans.ID,
//                    Name = ans.Name
//                }).ToList()
//                })

//vm.Questions = Questions.Select(x => new QuestionDto()
//{
//    ID = x.ID,
//    Name = x.Name,
//    Answers = db.Answers.Where(ans => ans.QuestionID == x.ID).Select(ans => new AnswerDto()
//    {
//        ID = ans.ID,
//        Name = ans.Name
//    }).ToList()
//}).ToList();



//vm.Question = Questions.Select(x=>new QuestionDto()
//{
//    ID = x.ID,
//    Name = x.Name,
//    Answers = db.Answers.Where(ans => ans.QuestionID == x.ID)
//    .Select(ans => new AnswerDto()
//    {
//        ID = ans.ID,
//        Name = ans.Name
//    }).ToList()
//}).OrderBy(x => x.ID).Skip(skip.Value).First();

//var rnd = new Random();
//var randomNumbers = Enumerable.Range(1, 6).OrderBy(x => rnd.Next()).Take(6).ToList();
//vm.QuestionIDS = randomNumbers;