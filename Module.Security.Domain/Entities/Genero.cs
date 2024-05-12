namespace Module.Security.Domain.Entities
{
    public class Genero
    {
        public int GenId { get; set; }
        public string? GesDescripcion { get; set; }
        public bool? GenActivo { get; set; }

        public string GenActivoDesc
        {
            get
            {
                return GenActivo == null ? "N/A" : GenActivo == true ? "ACTIVO" : "INACTIVO";
            }
        }
    }
}
