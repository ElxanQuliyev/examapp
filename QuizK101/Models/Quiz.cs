namespace QuizK101.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Quiz")]
    public partial class Quiz
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quiz()
        {
            QuizQuestions = new List<QuizQuestion>();
        }

        public int ID { get; set; }

        public int? ExamID { get; set; }

        [Required]
        [StringLength(128)]
        public string UserID { get; set; }

        public DateTime QuizStart { get; set; }

        public DateTime QuizEnd { get; set; }

        public TimeSpan QuizTime { get; set; }

        public int QuizCount { get; set; }

        public int? Score { get; set; }

        public bool QuizActive { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual List<QuizQuestion> QuizQuestions { get; set; }
    }
}
