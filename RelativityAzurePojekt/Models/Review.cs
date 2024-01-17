using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RelativityAzurePojekt.Models
{
    public class Review
    {
        [Key]
        public int? ID { get; set; }
        [ForeignKey("Movie")]
        public int MovieID { get; set; }
        [ForeignKey("AppUser")]
        public int AppUserID { get; set; }
        [Range(1, 5)]
        public int Stars { get; set; }
        public string? Opinion { get; set; }
    }
}
