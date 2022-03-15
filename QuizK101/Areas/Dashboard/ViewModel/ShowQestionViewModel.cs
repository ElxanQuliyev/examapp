using QuizK101.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizK101.Areas.Dashboard.ViewModel
{
    public class ShowQestionViewModel
    {
        public Exam Exam{ get; set; }
        public List<Question> Questions{ get; set; }
    }
}