namespace Planta.Domain.Constants;

/// <summary>Catalogos de la base de datos propia que se consultan por campania (idProyecto). Cada uno es un SP PLANTA_Listar*.</summary>
public enum CatalogoPorProyecto
{
    TiposProcesoEmpacado,
    CodigosRancho,
    LugaresProduccion,
    Conductores,
    Vehiculos,
    Transportistas,
    Supervisores,
    PersonalLogistico
}

/// <summary>Catalogos de la base de datos propia que se consultan por cultivo (codigoCultivo).</summary>
public enum CatalogoPorCultivo
{
    Formatos,
    TiposClamshell,
    TiposCaja,
    Presentaciones,
    TiposEmpaqueGuia,
    Categorias,
    TiposEmpaque
}
