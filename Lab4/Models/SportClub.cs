using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab4.Models
{
    public class SportClub
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Id")]
        [Required]
        public string ID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Fee { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; }
    }
}
