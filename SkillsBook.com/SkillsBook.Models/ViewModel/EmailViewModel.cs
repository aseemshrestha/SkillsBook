using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsBook.Models.DAL;

namespace SkillsBook.Models.ViewModel
{
    public class EmailViewModel
    {
        public string FromEmailAddress
        {
            get { return Constants.FromEmailAddress; }
        }

        public string ToEmailAddress { get; set; }

    }
}
