using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.Models
{
    public class News
    {
        public String Symbol { get; set; }
        public String Title { get; set; }
        public String PublishedDate { get; set; }
        public DateTime PublishedDateTime { get; set; }
        public String Link { get; set; }
    }
}
