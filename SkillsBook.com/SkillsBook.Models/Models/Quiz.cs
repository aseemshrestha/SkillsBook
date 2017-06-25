using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.Models
{
    [Table("Quiz")]
    public class QuizModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuizId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public DateTime AskedOn { get; set; }
        public string IpAddress { get; set; }
        public DateTime LastUpdated { get; set; }
        public int TotalAnswers { get; set; }
        public virtual IList<QuizAnswersModel> QuizAnswers { get; set; }

    }
    [Table("QuizAnswer")]
    public class QuizAnswersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }
        public int QuizQuizId { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string RightAnswer { get; set; }
        public string IpAddress { get; set; }
        public DateTime LastUpdated { get; set; }
        public virtual QuizModel Quiz { get; set; }
        

        // public virtual IList<AnswerResponseModel> AnswerResponse { get; set; }

    }
}
