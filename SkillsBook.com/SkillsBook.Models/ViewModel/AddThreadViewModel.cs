using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.ViewModel
{
    public class AddThreadViewModel
    {
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

        //classified
        public string ClassifiedTitle { get; set; }
        public string ClassifiedCategory { get; set; }
        public string HousingType { get; set; }
        public string BuySellItem { get; set; }
        public string ClassifiedImageLoc { get; set; }
        public double Price { get; set; }
        public string AdditionalInfo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ClassifiedRadio { get; set; }
        public string JobDetails { get; set; }

    }
}
