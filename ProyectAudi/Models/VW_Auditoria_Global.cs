using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProyectAudi.Models;

[Keyless]
public partial class VW_Auditoria_Global
{
    public int BITACORA_ID { get; set; }

    [StringLength(150)]
    public string USUARIO_EJECUTOR { get; set; } = null!;

    [StringLength(150)]
    public string? ENTIDAD_AFECTADA { get; set; }

    [StringLength(100)]
    public string NOMBRE_TABLA { get; set; } = null!;

    [StringLength(20)]
    public string TIPO_OPERACION { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime FECHA_OPERACION { get; set; }

    [StringLength(100)]
    public string CAMPO { get; set; } = null!;

    public string? VALOR_ANTERIOR { get; set; }

    public string? VALOR_NUEVO { get; set; }

    [StringLength(255)]
    public string? OBSERVACION { get; set; }
}
