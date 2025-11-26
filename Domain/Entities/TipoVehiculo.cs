using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_transporte.Domain.Entities
{
    [Table("TRANS_tipo_vehiculo")]
    public class TipoVehiculo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("vc_nombre")]
        public string? Nombre { get; set; } = string.Empty;

        [Column("vc_abreviatura")]
        public string? Abreviatura { get; set; } = string.Empty;

        [Column("b_estado")]
        public bool Estado { get; set; }

        [Column("user_cr")]
        public string? UsuarioCreacion { get; set; } = string.Empty;

        [Column("date_cr")]
        public DateTime? FechaCreacion { get; set; }
    }
}
