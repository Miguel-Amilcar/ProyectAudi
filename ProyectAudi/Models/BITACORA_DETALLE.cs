using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

public partial class BITACORA_DETALLE
{
    [Key]
    public int DETALLE_ID { get; set; }

    public int BITACORA_ID { get; set; }

    [StringLength(100)]
    public string CAMPO { get; set; } = null!;

    public string? VALOR_ANTERIOR { get; set; }

    public string? VALOR_NUEVO { get; set; }

    [ForeignKey("BITACORA_ID")]
    [InverseProperty("BITACORA_DETALLE")]
    public virtual BITACORA_ENCABEZADO BITACORA { get; set; } = null!;
}
