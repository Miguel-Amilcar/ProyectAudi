using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

[Index(nameof(USUARIO_CORREO), IsUnique = true)]
[Index(nameof(CUI), IsUnique = true)]
public partial class USUARIO
{
    [Key]
    public int USUARIO_ID { get; set; }

    [Unicode(false)]
    public string CUI { get; set; } = null!;

    public string PRIMERNOMBRE { get; set; } = null!;
    public string? SEGUNDONOMBRE { get; set; }
    public string? TERCERNOMBRE { get; set; }
    public string PRIMERAPELLIDO { get; set; } = null!;
    public string? SEGUNDOAPELLIDO { get; set; }
    public string? APELLIDOCASADA { get; set; }

    [Column(TypeName = "date")]
    public DateTime? FECHA_NACIMIENTO { get; set; }

    [Unicode(false)]
    public string? TELEFONO { get; set; }

    public string? DIRECCION { get; set; }
    [Unicode(false)]
    public string? PERSONA_NIT { get; set; }
    public string? PERSONA_DIRECCION { get; set; }
    [Unicode(false)]
    public string? PERSONA_TELEFONOCASA { get; set; }
    [Unicode(false)]
    public string? PERSONA_TELEFONOMOVIL { get; set; }

    [Column(TypeName = "varbinary(max)")]
    public byte[]? FOTOGRAFIA { get; set; }

    [Column(TypeName = "varbinary(max)")]
    public byte[]? DPI_PDF { get; set; }

    public string USUARIO_CORREO { get; set; } = null!;
    public byte ESTADO_TINY { get; set; }
    public int ROL_ID { get; set; }

    [Unicode(false)]
    public string CREADO_POR { get; set; } = null!;
    [Column(TypeName = "datetime")]
    public DateTime FECHA_CREACION { get; set; }

    [Unicode(false)]
    public string? MODIFICADO_POR { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? FECHA_MODIFICACION { get; set; }

    public bool ELIMINADO { get; set; }

    [Unicode(false)]
    public string? ELIMINADO_POR { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? FECHA_ELIMINACION { get; set; }

    [ForeignKey("ROL_ID")]
    public virtual ROL ROL { get; set; } = null!;
    public virtual CREDENCIAL CREDENCIAL { get; set; } = null!;
    public virtual TOKEN_RECUPERACION TOKEN_RECUPERACION { get; set; } = null!;
}

