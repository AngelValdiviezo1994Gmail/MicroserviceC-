using Common.Domain.Enums;


namespace Common.Domain.Entities
{
    public class OrdenFiltro
    {
        public string? PropiedadBusqueda { get; set; }
        public OrdenFiltroEnum Orden { get; set; }
    }
}
