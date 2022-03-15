using QuizK101.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizK101.Areas.Dashboard.ViewModel
{
    public class QuestAsnwerVM
    {
        public string QuestionName { get; set; }
        public List<Answer> Answers { get; set; }
        public int myAnswerID { get; set; }
        public int SubjectID { get; set; }
    }
}