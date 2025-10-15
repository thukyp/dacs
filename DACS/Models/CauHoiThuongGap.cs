using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    [Table("CauHoiThuongGap")]
    public class CauHoiThuongGap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string CauHoi { get; set; }

        [Required]
        [StringLength(255)]
        public string CauTraLoi { get; set; }
    }
}
