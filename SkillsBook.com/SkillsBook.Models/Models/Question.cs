using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.Models
{
    [Table("Questions")]
    public class QuestionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public DateTime AskedOn { get; set; }
        public string IpAddress { get; set; }
        public DateTime LastUpdated { get; set; }
        public int TotalAnswers { get; set; }
        public virtual IList<AnswerModel> Answers { get; set;}
        public virtual IList<AnswerResponseModel> AnswerResponse{ get; set; }
    

    }
    [Table("Answers")]
    public class AnswerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }
        public int QuestionQuestionId { get; set; }
        public string Answer { get; set; }
        public string AnsweredBy { get; set; }
        public DateTime AnsweredOn { get; set; }
        public string IpAddress { get; set; }
        public DateTime LastUpdated { get; set; }
        public int UsefulYes { get; set; }
        public int UsefulNo { get; set; }
        public int UserfulSomeWhat { get; set; }
        public virtual QuestionModel Question { get; set; }
       // public virtual IList<AnswerResponseModel> AnswerResponse { get; set; }

    }
    [Table("AnswerResponse")]
    public class AnswerResponseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerResponseId { get; set; }
        public int AnswerAnswerId { get; set; }
        public string RatedBy { get; set; }
        public DateTime RatedOn { get; set; }
        public string IpAddress { get; set; }
        public virtual QuestionModel Question { get; set; }
    }

}
