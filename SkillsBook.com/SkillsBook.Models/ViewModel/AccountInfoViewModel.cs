using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.ViewModel
{
   public class AccountInfoViewModel
    {
       public int ThreadId { get; set; }
       public string Email { get; set; }
       public string Title { get; set; }
       public string ThreadRadio { get; set; }
       public string Announcement{ get; set; }
       public int Likes { get; set; }
       public int Responses { get; set; }
       public int Views { get; set; }
       public int Watch { get; set; }
       public DateTime CreatedOn { get; set; }
       public DateTime SubmittedOn { get; set; }
       public DateTime LastSuccessfulLogin { get; set; }
       public DateTime CommentedOn { get; set; }
       public DateTime LikedOn { get; set; }
       public DateTime ViewedOn { get; set; }
       public DateTime WatchedOn { get; set; }

    }
}
