using System.Text.Json.Nodes;

namespace Planta.Infrastructure.ServiceImpl;

/// <summary>
/// ApiDocumentosElectronicos espera la guia con nombres de propiedad en PascalCase y algunos nombres propios
/// (por ejemplo "ruc_Transportistas" -> RucTransportista). Esta tabla es el contrato con esa API.
/// </summary>
internal static class GuiaParaDocumentosElectronicos
{
    private static readonly Dictionary<string, string> Nombres = new(StringComparer.OrdinalIgnoreCase)
    {
        ["id"] = "Id",
        ["idempresa"] = "IdEmpresa",
        ["numeroDocumentoRemitente"] = "NumeroDocumentoRemitente",
        ["razonSocialRemitente"] = "RazonSocialRemitente",
        ["idProyecto"] = "IdProyecto",
        ["codigoAcopio"] = "CodigoAcopio",
        ["codigoGuiaRemision"] = "CodigoGuiaRemision",
        ["serie"] = "Serie",
        ["numero"] = "Numero",
        ["correoEmisor"] = "CorreoEmisor",
        ["correoAdquiriente"] = "CorreoAdquiriente",
        ["codigoProceso"] = "CodigoProceso",
        ["nombreProceso"] = "NombreProceso",
        ["documentoDestinatario"] = "DocumentoDestinatario",
        ["nombreDestinatario"] = "NombreDestinatario",
        ["tipoDocumentoRemitente"] = "TipoDocumentoRemitente",
        ["tipoDocumentoDestinatario"] = "TipoDocumentoDestinatario",
        ["puntoPartida"] = "PuntoPartida",
        ["puntoLlegada"] = "PuntoLlegada",
        ["ubigeoPartida"] = "UbigeoPartida",
        ["ubigeoLlegada"] = "UbigeoLlegada",
        ["idEstablecimientoPartida"] = "IdEstablecimientoPartida",
        ["idEstablecimientoLlegada"] = "IdEstablecimientoLlegada",
        ["descripcionEstablecimientoPartida"] = "DescripcionEstablecimientoPartida",
        ["codigoEstablecimientoSunatPartida"] = "CodigoEstablecimientoSunatPartida",
        ["idTipoEstablecimientoPartida"] = "IdTipoEstablecimientoPartida",
        ["descripcionEstablecimientoLlegada"] = "DescripcionEstablecimientoLlegada",
        ["codigoEstablecimientoSunatLlegada"] = "CodigoEstablecimientoSunatLlegada",
        ["idTipoEstablecimientoLlegada"] = "IdTipoEstablecimientoLlegada",
        ["fechaEmision"] = "FechaEmision",
        ["idTransportista"] = "IdTransportista",
        ["razonSocialTransportista"] = "RazonSocialTransportista",
        ["RucTransportista"] = "RucTransportista",
        ["ruc_Transportistas"] = "RucTransportista",
        ["tipoDocumentoTransportista"] = "TipoDocumentoTransportista",
        ["idConductor"] = "IdConductor",
        ["NombreConductor"] = "NombreConductor",
        ["ApellidoConductor"] = "ApellidoConductor",
        ["nombreCompletoConductor"] = "NombreCompletoConductor",
        ["DniConductor"] = "DniConductor",
        ["numeroLicencia"] = "NumeroLicencia",
        ["tipoDocumentoConductor"] = "TipoDocumentoConductor",
        ["idVehiculo"] = "IdVehiculo",
        ["placaPrincipalVehiculo"] = "PlacaPrincipalVehiculo",
        ["placaRemolque"] = "PlacaRemolque",
        ["certificadoInscripcion"] = "CertificadoInscripcion",
        ["codigoSunatMotivoTraslado"] = "CodigoSunatMotivoTraslado",
        ["descripcionMotivoTraslado"] = "DescripcionMotivoTraslado",
        ["precinto"] = "Precinto",
        ["inicioTraslado"] = "InicioTraslado",
        ["observaciones"] = "Observaciones",
        ["observacionGuia"] = "ObservacionGuia",
        ["estado"] = "Estado",
        ["estadoSunat"] = "EstadoSunat",
        ["codigoEstadoSunat"] = "CodigoEstadoSunat",
        ["pesoTotal"] = "PesoTotal",
        ["unidadMedidaPesoBruto"] = "UnidadMedidaPesoBruto",
        ["totalCajas"] = "TotalCajas",
        ["cantidadPalets"] = "CantidadPalets",
        ["usuarioEmision"] = "UsuarioEmision",
        ["fechaCreacionWeb"] = "FechaCreacionWeb",
        ["fechaCierre"] = "FechaCierre",
        ["parihuelas"] = "Parihuelas",
        ["observacionesUsuario"] = "ObservacionesUsuario",
        ["esReposicion"] = "EsReposicion",
        ["inspeccionTemperatura"] = "InspeccionTemperatura",
        ["inspeccionLibreOlores"] = "InspeccionLibreOlores",
        ["inspeccionLibreInsectos"] = "InspeccionLibreInsectos",
        ["inspeccionLibreMateriasExtranas"] = "InspeccionLibreMateriasExtranas",
        ["inspeccionUnidadLimpia"] = "InspeccionUnidadLimpia",
        ["inspeccionObservaciones"] = "InspeccionObservaciones",
        ["inspeccionMedidaCorrectiva"] = "InspeccionMedidaCorrectiva",
        ["numeroViaje"] = "NumeroViaje",
        ["fechaEntregaBienes"] = "FechaEntregaBienes",
        ["esEnsayo"] = "EsEnsayo",
        ["transactionId_uuid"] = "TransactionIdUuid",
        ["fechaCreacion"] = "FechaCreacion",
        ["usuarioCre"] = "UsuarioCre",
        ["usuarioMod"] = "UsuarioMod",
        ["fechaModificacion"] = "FechaModificacion",
        ["modalidadTraslado"] = "ModalidadTraslado",
        ["modalidadTrasladoDescripcion"] = "ModalidadTrasladoDescripcion",
        ["detalle"] = "Detalle",
        ["codigoPalet"] = "CodigoPalet",
        ["codigoDPalet"] = "CodigoDPalet",
        ["codigoItem"] = "CodigoItem",
        ["codigoTipoProcesoEmpacado"] = "CodigoTipoProcesoEmpacado",
        ["cantidadCajas"] = "CantidadCajas",
        ["unidadMedida"] = "UnidadMedida",
        ["documentoConsignatario"] = "DocumentoConsignatario",
        ["idDestino"] = "IdDestino",
        ["codigoFormato"] = "CodigoFormato",
        ["codigoVariedad"] = "CodigoVariedad",
        ["idTipoEmpaqueGuia"] = "IdTipoEmpaqueGuia",
        ["idCodigoRancho"] = "IdCodigoRancho",
        ["idLugarProduccion"] = "IdLugarProduccion",
        ["idPresentacion"] = "IdPresentacion",
        ["pesoPorCaja"] = "PesoPorCaja",
        ["idTransporte"] = "IdTransporte",
        ["nombreTipoProcesoEmpacado"] = "NombreTipoProcesoEmpacado",
        ["nombrePresentacion"] = "NombrePresentacion",
        ["nombreFormato"] = "NombreFormato",
        ["nombreTipoEmpaqueGuia"] = "NombreTipoEmpaqueGuia",
        ["codigoLugarProduccion"] = "CodigoLugarProduccion",
        ["codigoRancho"] = "CodigoRancho",
        ["nombreConsignatario"] = "NombreConsignatario",
        ["nombreDestino"] = "NombreDestino",
        ["nombreVariedad"] = "NombreVariedad",
        ["nombreTransporte"] = "NombreTransporte",
        ["detalleDescripcion"] = "DetalleDescripcion"
    };

    /// <summary>Copia la guia (y sus listas anidadas) cambiando los nombres de propiedad al formato de esa API.</summary>
    public static JsonNode? Convertir(JsonNode? nodo)
    {
        switch (nodo)
        {
            case JsonObject objeto:
                var resultado = new JsonObject();
                foreach (var propiedad in objeto)
                {
                    var nombre = Nombres.TryGetValue(propiedad.Key, out var mapeado) ? mapeado : propiedad.Key;
                    resultado[nombre] = Convertir(propiedad.Value);
                }

                return resultado;
            case JsonArray arreglo:
                var lista = new JsonArray();
                foreach (var item in arreglo)
                {
                    lista.Add(Convertir(item));
                }

                return lista;
            default:
                return nodo?.DeepClone();
        }
    }
}
