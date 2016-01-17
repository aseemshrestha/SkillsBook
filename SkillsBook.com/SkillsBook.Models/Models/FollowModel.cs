using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.Models
{
    [Table("Follow")]
    public class FollowModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FollowId { get; set; }
        public int FollowedBy { get; set; }
        public string Following { get; set; }
        public DateTime FollowedOn { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }

    }
}
