using Common.Domain;
using Common.Domain.Entities;
using Common.Domain.Entities.Persistence;
using Common.Domain.Interfaces;


namespace Common.Infrastructure.Helpers
{
    public class SesionHelper : ISesionHelper
    {
        private DatosSession? _session { get; set; } = null;

        public void SetDatos(DatosSession datos)
        {
            //if(_session == null)
            _session = datos;
        }

        public bool IsSesionValid()
        {
            return _session != null;
        }

        public DatosSession GetAll()
        {
            return _session;
        }

        public void KillSession()
        {
            _session = null;
        }

        public int GetUsuarioId()
        {
            return _session != null ? _session.UsuarioId : 0;
        }

        public string GetIdentificador()
        {

            return _session != null ? _session.Identificador.ToString() : string.Empty;

        }

        public string GetNombreCompleto()
        {
            return _session != null ? $"{_session.PrimerNombre} {_session.SegundoNombre} {_session.PrimerApellido} {_session.SegundoApellido}" : string.Empty;
        }

        public string GetNombreCorto()
        {
            return _session != null ? $"{_session.PrimerNombre} {_session.PrimerApellido}" : string.Empty;
        }

        public string GetUsuario()
        {
            return _session != null ? _session.Usuario : string.Empty;
        }
        public string GetCorreoInstitucional()
        {
            return _session != null ? _session.CorreoInstitucional : string.Empty;
        }

        public int GetEscuela()
        {
            return _session != null ? _session.EscId : 0;
        }

        public string GetEscuelaLogo()
        {
            var realPath = _session.EscLogo ?? "";
            var sLogo = _session != null ? $"/Logos/{_session.EscId}{Path.GetExtension(_session.EscLogo)}" : "/recursos/2. Logo Armanda/SVG/Recurso 9.svg";

            if (!System.IO.File.Exists(sLogo) && realPath.StartsWith("/recursos"))
            {
                sLogo = realPath;
            }

            return sLogo;
        }

        public string GetEscuelaNombre()
        {
            return _session != null ? _session.EscName : "Administración";
        }

        public string GetEscuelaNombreCorto()
        {
            return _session != null ? _session.EscNameExt : "Administración";
        }

        public string GetRolesUsuario()
        {
            return _session != null ? _session.LstRoles : string.Empty;
        }

        public string GetPermisosRolesUsuario()
        {
            return _session != null ? _session.LstPermisosRoles : string.Empty;
        }

        public bool IsRolUserSuperAdmin()
        {
            return _session != null ? Convert.ToBoolean(_session.IsSuperAdmin) : false;
        }
    }
}
