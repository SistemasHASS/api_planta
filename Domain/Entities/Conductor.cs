using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_transporte.Domain.Entities
{
    [Table("TRANS_conductor")]
    public class Conductor
    {
        [Key]
        [Column("id_conductor")]
        public int Id { get; set; }

        [Column("vc_nombres")]
        public string Nombres { get; set; } = string.Empty;

        [Column("vc_apellido_paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [Column("vc_apellido_materno")]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [Column("vc_dni")]
        public string Dni { get; set; } = string.Empty;

        [Column("vc_nro_licencia")]
        public string NumeroLicencia { get; set; } = string.Empty;

        [Column("i_tipo_brevete")]
        public int TipoBrevete { get; set; }

        [Column("d_fv_licencia")]
        public DateTime? FechaVencLicencia { get; set; }

        [Column("d_fv_sctr")]
        public DateTime? FechaVencSctr { get; set; }

        [Column("id_transportista")]
        public int IdTransportista { get; set; }

        [Column("ruc_cliente")]
        public string? RucCliente { get; set; }

        [Column("i_empresa_id")]
        public string? EmpresaId { get; set; }

        [Column("i_estado_conductor")]
        public int EstadoConductor { get; set; }
    }
}
