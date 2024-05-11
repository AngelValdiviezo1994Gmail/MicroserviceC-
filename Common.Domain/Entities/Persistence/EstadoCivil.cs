
namespace Common.Domain.Entities.Persistence
{
    public partial class EstadoCivil
    {
        public int Id { get; set; }
        public string? EstCivDescripcion { get; set; }
        public bool? EstCivActivo { get; set; }

        public string EstCivActivoDesc
        {
            get
            {
                return EstCivActivo == null ? "N/A" : EstCivActivo == true ? "ACTIVO" : "INACTIVO";
            }
        }
    }
}
