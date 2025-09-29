using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

[Index("USUARIO_CORREO", Name = "UQ__USUARIO__2760835EA60D8CBC", IsUnique = true)]
[Index("CUI", Name = "UQ__USUARIO__C1F8BFD0B6C69FB1", IsUnique = true)]
public partial class USUARIO
{
    [Key]
    public int USUARIO_ID { get; set; }

    [StringLength(13)]
    [Unicode(false)]
    public string? CUI { get; set; }

    [StringLength(50)]
    public string PRIMERNOMBRE { get; set; } = null!;

    [StringLength(50)]
    public string? SEGUNDONOMBRE { get; set; }

    [StringLength(50)]
    public string? TERCERNOMBRE { get; set; }

    [StringLength(50)]
    public string PRIMERAPELLIDO { get; set; } = null!;

    [StringLength(50)]
    public string? SEGUNDOAPELLIDO { get; set; }

    [StringLength(50)]
    public string? APELLIDOCASADA { get; set; }

    public DateOnly? FECHA_NACIMIENTO { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? TELEFONO { get; set; }

    [StringLength(200)]
    public string? DIRECCION { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? PERSONA_NIT { get; set; }

    [StringLength(255)]
    public string? PERSONA_DIRECCION { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? PERSONA_TELEFONOCASA { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? PERSONA_TELEFONOMOVIL { get; set; }

    public byte[]? FOTOGRAFIA { get; set; }

    public byte[]? DPI_PDF { get; set; }

    [StringLength(150)]
    public string USUARIO_CORREO { get; set; } = null!;

    public byte ESTADO_TINY { get; set; }

    public int ROL_ID { get; set; }

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

    [InverseProperty("USUARIO")]
    public virtual ICollection<BITACORA_ENCABEZADO> BITACORA_ENCABEZADOUSUARIO { get; set; } = new List<BITACORA_ENCABEZADO>();

    [InverseProperty("USUARIO_AFECTADO")]
    public virtual ICollection<BITACORA_ENCABEZADO> BITACORA_ENCABEZADOUSUARIO_AFECTADO { get; set; } = new List<BITACORA_ENCABEZADO>();

    [InverseProperty("USUARIO")]
    public virtual ICollection<CREDENCIAL> CREDENCIAL { get; set; } = new List<CREDENCIAL>();

    [InverseProperty("USUARIO")]
    public virtual ICollection<ROL> ROL { get; set; } = new List<ROL>();
}
