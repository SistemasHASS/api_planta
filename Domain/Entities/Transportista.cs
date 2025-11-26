using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_transporte.Domain.Entities
{
    [Table("TRANS_transportista")]
    public class Transportista
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("vc_razonSocial")]
        public string? RazonSocial { get; set; } = string.Empty;

        [Column("nombrecorto")]
        public string? NombreCorto { get; set; } = string.Empty;

        [Column("vc_ruc")]
        public string? Ruc { get; set; } = string.Empty;

        [Column("vc_telefono")]
        public string? Telefono { get; set; } = string.Empty;

        [Column("i_estado_transportista")]
        public int EstadoTransportista { get; set; }
    }
}
