using ProyectAudi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class USUARIO
{
    [Key] public int USUARIO_ID { get; set; }

    // Datos personales (ya definidos en tu DB)
    [MaxLength(13)] public string? CUI { get; set; }
    [Required, MaxLength(50)] public string PRIMERNOMBRE { get; set; } = null!;
    [MaxLength(50)] public string? SEGUNDONOMBRE { get; set; }
    [MaxLength(50)] public string? TERCERNOMBRE { get; set; }
    [Required, MaxLength(50)] public string PRIMERAPELLIDO { get; set; } = null!;
    [MaxLength(50)] public string? SEGUNDOAPELLIDO { get; set; }
    [MaxLength(50)] public string? APELLIDOCASADA { get; set; }
    public DateTime? FECHA_NACIMIENTO { get; set; }
    [MaxLength(20)] public string? TELEFONO { get; set; }
    [MaxLength(200)] public string? DIRECCION { get; set; }

    [MaxLength(20)] public string? PERSONA_NIT { get; set; }
    [MaxLength(255)] public string? PERSONA_DIRECCION { get; set; }
    [MaxLength(20)] public string? PERSONA_TELEFONOCASA { get; set; }
    [MaxLength(20)] public string? PERSONA_TELEFONOMOVIL { get; set; }

    public byte[]? FOTOGRAFIA { get; set; }
    public byte[]? DPI_PDF { get; set; }

    [Required, MaxLength(150)] public string USUARIO_CORREO { get; set; } = null!;
    // 0=Inactivo, 1=Activo, 2=Bloqueado (por ejemplo)
    [Range(0, 2)] public byte ESTADO_TINY { get; set; } = 1;

    // Rol (FK)
    public int ROL_ID { get; set; }
    public ROL ROL { get; set; } = null!;

    // Auditoría
    [Required, MaxLength(50)] public string CREADO_POR { get; set; } = null!;
    public DateTime FECHA_CREACION { get; set; } = DateTime.Now;
    [MaxLength(50)] public string? MODIFICADO_POR { get; set; }
    public DateTime? FECHA_MODIFICACION { get; set; }
    public bool ELIMINADO { get; set; } = false;
    [MaxLength(50)] public string? ELIMINADO_POR { get; set; }
    public DateTime? FECHA_ELIMINACION { get; set; }

    // Relaciones

    [ForeignKey(nameof(ROL_ID))]
    public virtual ROL ROL { get; set; } = null!;

    public virtual CREDENCIAL CREDENCIAL { get; set; } = null!;

    public virtual ICollection<BITACORA_ENCABEZADO> BITACORA_ENCABEZADOUSUARIO { get; set; } = new List<BITACORA_ENCABEZADO>();

    public virtual ICollection<BITACORA_ENCABEZADO> BITACORA_ENCABEZADOUSUARIO_AFECTADO { get; set; } = new List<BITACORA_ENCABEZADO>();

    public virtual TOKEN_RECUPERACION TOKEN_RECUPERACION { get; set; } = null!;

}
