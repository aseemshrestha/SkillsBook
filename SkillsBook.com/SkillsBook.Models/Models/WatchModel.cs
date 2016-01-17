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
    
    [Table("Watch")]
    public class WatchModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WatchId { get; set; }
        public int ThreadId { get; set; }
        public string WatchedBy { get; set; }
        public DateTime WatchedOn { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }

    }
}
