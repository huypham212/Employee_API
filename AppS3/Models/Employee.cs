using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppS3.Models
{
    public class Employee 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DayGetIn { get; set; } = DateTime.Now;

        [Required]
        public double SalScale1 { get; set; }

        [Required]
        [ForeignKey("PositionId")]
        public int PositionId { get; set; }
    }
}
