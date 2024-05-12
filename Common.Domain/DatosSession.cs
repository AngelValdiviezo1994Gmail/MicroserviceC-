
namespace Common.Domain
{
    public class DatosSession
    {
        public Guid Identificador { get; set; }
        public int UsuarioId { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? CorreoInstitucional { get; set; }
        public string? Usuario { get; set; }
        public int EscId { get; set; }
        public string EscLogo { get; set; }
        public string EscName { get; set; }
        public string EscNameExt { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }

        public string LstRoles { get; set; } = string.Empty;
        public string LstPermisosRoles { get; set; } = string.Empty;

        public string IsSuperAdmin { get; set; } = string.Empty;

    }
}
