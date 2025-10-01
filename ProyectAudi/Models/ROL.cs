using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

[Index("ROL_NOMBRE", Name = "UQ__ROL__9D60F34C3206F53D", IsUnique = true)]
public partial class ROL
{
    [Key]
    public int ROL_ID { get; set; }

    [StringLength(100)]
    public string ROL_NOMBRE { get; set; } = null!;

    [StringLength(255)]
    public string? ROL_DESCRIPCION { get; set; }

    public bool ES_ADMINISTRATIVO { get; set; }

    public bool ESTADO { get; set; }

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

    [StringLength(50)]
    [Unicode(false)]
    public string? ELIMINADO_POR { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FECHA_ELIMINACION { get; set; }

    public int USUARIO_ID { get; set; }

    [InverseProperty("ROL")]
    public virtual ICollection<PERMISO> PERMISO { get; set; } = new List<PERMISO>();

    [ForeignKey("USUARIO_ID")]
    public virtual ICollection<USUARIO> USUARIOS { get; set; } = new List<USUARIO>();



}
