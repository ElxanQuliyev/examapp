using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuizK101.Areas.Dashboard.ViewModel
{
    public class CreateQuestionAnswer
    {
        [Required]
        public string QuestionName { get; set; }

        [Required]
        public string[] answers { get; set; }
        [Required]
        public int? ExamID { get; set; }
        public int? SubjectID { get; set; }

    }
}