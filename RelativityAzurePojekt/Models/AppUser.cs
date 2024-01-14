using System.ComponentModel.DataAnnotations;

namespace RelativityAzurePojekt.Models
{
    public class AppUser
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Passwd { get; set; } = string.Empty;

    }
}
