using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectAudi.Models
{
    [Table("ROL_PERMISO")]
    public class ROL_PERMISO
    {
        [Key, Column(Order = 0)]
        [ForeignKey("ROL")]
        public int ROL_ID { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("PERMISO")]
        public int PERMISO_ID { get; set; }

        public virtual ROL ROL { get; set; } = null!;
        public virtual PERMISO PERMISO { get; set; } = null!;
    }
}