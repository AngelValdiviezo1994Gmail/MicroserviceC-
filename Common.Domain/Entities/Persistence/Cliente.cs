
namespace Common.Domain.Entities.Persistence
{
    public partial class Cliente
    {
        public int ClientId { get; set; }
        public string? ClientNombre { get; set; }
        public string? ClientApellido { get; set; }
        public string? ClientNumCta { get; set; }
        public int? ClientSaldo { get; set; }
        public DateTime? ClientFechaNacimiento { get; set; }
        public string? ClientDireccion { get; set; }
        public string? ClientTelefono { get; set; }
        public string? ClientEmail { get; set; }
        public int? ClientTipoId { get; set; }
        public int? ClientEstadoCivilId { get; set; }
        public string? ClientNumIdentificacion { get; set; }
        public string? ClientProfesion { get; set; }
        public int? ClientGeneroId { get; set; }
        public string? ClientNacionalidad { get; set; }
    }
}
