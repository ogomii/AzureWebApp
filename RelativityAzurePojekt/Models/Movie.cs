using System;
using System.ComponentModel.DataAnnotations;

namespace RelativityAzurePojekt.Models
{
    public class Movie
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
    }
}