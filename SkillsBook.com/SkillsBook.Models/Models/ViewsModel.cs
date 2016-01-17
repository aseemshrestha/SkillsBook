using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.Models
{
   
        [Table("Views")]
        public class ViewsModel
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ViewId { get; set; }
            public int ThreadId { get; set; }
            public string ViewedBy { get; set; }
            public DateTime ViewedOn { get; set; }
            public string IpAddress { get; set; }
            public string Browser { get; set; }

        }
    
}
