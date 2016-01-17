using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.Models
{
    [Table("Comment")]
    public class CommentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public int Thread_ThreadId { get; set; }
        public string CommentedBy { get; set; }
        public DateTime CommentedOn { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public string Comment { get; set; }
        public virtual ThreadModel Thread { get; set; }

    }
}
