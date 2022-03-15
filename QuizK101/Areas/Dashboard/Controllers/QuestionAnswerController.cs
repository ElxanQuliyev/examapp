using QuizK101.Areas.Dashboard.ViewModel;
using QuizK101.Helpers;
using QuizK101.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QuizK101.Areas.Dashboard.Controllers
{
    public class QuestionAnswerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Dashboard/QuestionAnswer
        public ActionResult Index()
        {
            return View(db.Exams.ToList());
        }
        [HttpPost]
        public ActionResult selectExam(int? subjectID)
        {
            object result = null;
            if (subjectID == null)
            {
                result = new
                {
                    status = 404,
                    message = "Id Not Found",
                    response = ""
                };
            }
            var examList = db.Exams.Where(ex => ex.SubjectID == subjectID).Select(ex => new
            {
                ex.ID,
                ex.Name
            }).ToList();
            result = new
            {
                status = 200,
                message = "success",
                response = examList

            };

            return Json(result);
        }
        public ActionResult ShowQuestionExam(int? id)
        {
            if (id == null) return HttpNotFound();
            Exam selectedExam = db.Exams.FirstOrDefault(f => f.ID == id);
            if (selectedExam == null) return HttpNotFound();
            List<Question> questions = db.Questions.Where(q => q.ExamID == id).ToList();
            ShowQestionViewModel editVm = new ShowQestionViewModel
            {
                Questions = questions,
                Exam = selectedExam
            };
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName");
            return View(editVm);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null) return HttpNotFound();
            Question selectedQuestion = db.Questions.FirstOrDefault(q => q.ID == id);
            if (selectedQuestion == null) return HttpNotFound();
            ViewBag.ExamID = new SelectList(db.Exams, "ID", "Name");
            EditQuestionViewModel editQuestion = new EditQuestionViewModel
            {
                Question = selectedQuestion
            };
            return View(editQuestion);
        }

        [HttpPost]
        public ActionResult Edit(EditQuestionViewModel questionAnswer)
        {
            ViewBag.ExamID = new SelectList(db.Exams, "ID", "Name");
            if (ModelState.IsValid)
            {
                Question selectedQuestion = db.Questions.First(x => x.ID == questionAnswer.Question.ID); bool isSelectedAns = false;
                var oldAnswerList = db.Answers.Where(a => a.QuestionID == questionAnswer.Question.ID).ToList();
                if (questionAnswer.Answers != null && questionAnswer.Answers.Count > 0)
                {
                    foreach (var value in questionAnswer.Answers)
                    {
                        var ansTrue = value.GetSubstringText("~", "");
                        if (!string.IsNullOrEmpty(ansTrue))
                        {
                            isSelectedAns = true;
                            break;
                        }
                    }
                    if (isSelectedAns)
                    {

                        var answerList = new List<Answer>();

                        db.Answers.RemoveRange(oldAnswerList);
                        foreach (var value in questionAnswer.Answers)
                        {
                            var ansValue = value.GetSubstringText("", "~");
                            var ansTrue = value.GetSubstringText("~", "");
                            Answer answer = new Answer();
                            answer.Name = ansValue;
                            answer.QuestionID = questionAnswer.Question.ID;
                            db.Answers.Add(answer);
                            db.SaveChanges();
                            if (!string.IsNullOrEmpty(ansTrue))
                            {
                                selectedQuestion.CorrectAnswerID = answer.ID;
                                //db.Entry(selectedQuestion).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        return RedirectToAction("Index");
                    }

                }
            }

            return View(questionAnswer);
        }
        public ActionResult Create()
        {
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateQuestionAnswer questionAnswer)
        {

            if (ModelState.IsValid)
            {
                bool isSelectedAns = false;
                foreach (var value in questionAnswer.answers)
                {
                    var ansTrue = value.GetSubstringText("~", "");
                    if (!string.IsNullOrEmpty(ansTrue))
                    {
                        isSelectedAns = true;
                        break;
                    }
                }
                if (isSelectedAns)
                {
                    Question question = new Question();
                    question.Name = questionAnswer.QuestionName;
                    question.ExamID = questionAnswer.ExamID;
                    db.Questions.Add(question);
                    db.SaveChanges();
                    var answerList = new List<Answer>();
                    foreach (var value in questionAnswer.answers)
                    {

                        var ansValue = value.GetSubstringText("", "~");
                        var ansTrue = value.GetSubstringText("~", "");
                        Answer answer = new Answer();
                        answer.Name = ansValue;
                        answer.QuestionID = question.ID;
                        db.Answers.Add(answer);
                        db.SaveChanges();
                        if (!string.IsNullOrEmpty(ansTrue))
                        {
                            question.CorrectAnswerID = answer.ID;
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Index");
                }

            }
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "SubjectName", questionAnswer.SubjectID);

            return View();

        }

    }
}