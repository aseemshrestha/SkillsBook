using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.Models
{
    [Table("Threads")]
    public class ThreadModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ThreadId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string ThreadRadio { get; set; }
        //announcement
        public string Announcement { get; set; }
        public string TagsAnnouncement { get; set; }

        //general
        public string Title { get; set; }
        public string Content { get; set; }
        public string TagsGeneral { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public DateTime SubmittedOn { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Likes { get; set; }
        public int Views { get; set; }
        public int Responses { get; set; }
        public int Watch { get; set; }
        public virtual ICollection<CommentModel> Comments { get; set; }
    }
}
