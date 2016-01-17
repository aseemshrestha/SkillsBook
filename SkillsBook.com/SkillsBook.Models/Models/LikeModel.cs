using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.Models
{
    [Table("Likes")]
    public class LikeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LikeId { get; set; }
        public int ThreadId { get; set; }
        public string LikedBy { get; set; }
        public DateTime LikedOn { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }

    }
}
