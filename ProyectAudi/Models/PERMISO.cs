using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

[Index("PERMISO_NOMBRE", Name = "UQ__PERMISO__276F401A82147467", IsUnique = true)]
public partial class PERMISO
{
    [Key]
    public int PERMISO_ID { get; set; }

    [StringLength(100)]
    public string PERMISO_NOMBRE { get; set; } = null!;

    [StringLength(255)]
    public string? DESCRIPCION { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CREADO_POR { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FECHA_CREACION { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? MODIFICADO_POR { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FECHA_MODIFICACION { get; set; }

    public bool ELIMINADO { get; set; }

    public int ROL_ID { get; set; }

    [ForeignKey("ROL_ID")]
    [InverseProperty("PERMISO")]
    public virtual ROL ROL { get; set; } = null!;
}
