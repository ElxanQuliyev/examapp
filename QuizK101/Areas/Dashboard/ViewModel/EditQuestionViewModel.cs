using QuizK101.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizK101.Areas.Dashboard.ViewModel
{
    public class EditQuestionViewModel
    {

        public Question Question{ get; set; }
        public List<string> Answers { get; set; }
    }
}