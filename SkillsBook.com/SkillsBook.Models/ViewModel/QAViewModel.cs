using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsBook.Models.Models;

namespace SkillsBook.Models.ViewModel
{
    public class QaViewModel
    {
        public int QuestionId { get; set; }
        public string Username { get; set; }
        public string Answer { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Question { get; set; }
        public DateTime AskedOn { get; set; }
        public string AnsweredBy { get; set; }
        public DateTime AnsweredOn { get; set; }
        public int UsefulYes { get; set; }
        public int UsefulNo { get; set; }
        public int UserfulSomeWhat { get; set; }
    }
}
