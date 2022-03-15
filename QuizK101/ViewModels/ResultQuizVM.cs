using QuizK101.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizK101.ViewModels
{
    public class ResultQuizVM
    {
        public List<Quiz> Quizzes{ get; set; }
        public Quiz QuizeSingle { get; set; }

        public int Score { get; set; }
        public List<QuizQuestion>Questions { get; set; }
    }
}