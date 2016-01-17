using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.Models
{
   [Table("Classifieds")]
    public class ClassifiedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassifiedId { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
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
        public string IpAddress { get; set; }
        public DateTime PostedOn { get; set; }
        public string Browser { get; set; }
        public DateTime LastUpdated { get; set; }
        public int CurrentStatus { get; set; }
    }
}
