using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

public partial class BITACORA_ENCABEZADO
{
    [Key]
    public int BITACORA_ID { get; set; }

    public int USUARIO_ID { get; set; }

    public int? USUARIO_AFECTADO_ID { get; set; }

    public int TABLA_ID { get; set; }

    public int OPERACION_ID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FECHA_OPERACION { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? IP_ORIGEN { get; set; }

    [StringLength(255)]
    public string? OBSERVACION { get; set; }

    [InverseProperty("BITACORA")]
    public virtual ICollection<BITACORA_DETALLE> BITACORA_DETALLE { get; set; } = new List<BITACORA_DETALLE>();

    [ForeignKey("OPERACION_ID")]
    [InverseProperty("BITACORA_ENCABEZADO")]
    public virtual OPERACION OPERACION { get; set; } = null!;

    [ForeignKey("TABLA_ID")]
    [InverseProperty("BITACORA_ENCABEZADO")]
    public virtual TABLAS_AUDITABLE TABLA { get; set; } = null!;

    [ForeignKey("USUARIO_ID")]
    [InverseProperty("BITACORA_ENCABEZADOUSUARIO")]
    public virtual USUARIO USUARIO { get; set; } = null!;

    [ForeignKey("USUARIO_AFECTADO_ID")]
    [InverseProperty("BITACORA_ENCABEZADOUSUARIO_AFECTADO")]
    public virtual USUARIO? USUARIO_AFECTADO { get; set; }
}
