namespace QuizK101.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QuizQuestion
    {
        public int ID { get; set; }

        public int questionID { get; set; }

        public int quizID { get; set; }

        public int? myanswerID { get; set; }

        public virtual Question Question { get; set; }

        public virtual Quiz Quiz { get; set; }
    }
}
