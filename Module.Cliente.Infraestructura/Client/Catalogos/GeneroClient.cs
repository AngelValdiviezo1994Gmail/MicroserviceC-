using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using Module.Cliente.Domain.CQRS.Command;
using Module.Cliente.Domain.CQRS.Query;
using Module.Cliente.Domain.Entities;
using Module.Cliente.Infrastructura.Interfaces.Client;

namespace Module.Cliente.Infrastructura.Client
{
    public class GeneroClient : IGeneroClient
    {
        #region Propiedades

        private readonly IExecutionOrchestrator _executor;
        #endregion

        #region Constructor
        public GeneroClient(IExecutionOrchestrator executor) => (_executor) = (executor);

        #endregion

        #region Funciones
        public async Task<ResponsePaged<List<Genero>>> GetGeneros(RequestPaged request)
        => await _executor.ProcessCommandRequest<GetGeneros, ResponsePaged<List<Genero>>>(
            new GetGeneros
            {
                TrackingId = request.TrackingId,
                Cantidad = request.Cantidad,
                Pagina = request.Pagina,
                Filtros = request.Data,
                Orden = request.Orden

            }
        );

        public async Task<Response<Genero>> Registrar(Request<Genero> request)
        => await _executor.ProcessCommandRequest<RegistrarGeneroCommand, Response<Genero>>(
            _executor.Mapper.Map<RegistrarGeneroCommand>(request.Data)
            );

        public async Task<Response<Genero>> Modificar(Request<Genero> request)
        => await _executor.ProcessCommandRequest<ModificarGeneroCommand, Response<Genero>>(
            _executor.Mapper.Map<ModificarGeneroCommand>(request.Data)
            );

        public async Task<Response<bool>> Inactivar(Request<int> request)
        => await _executor.ProcessCommandRequest<InactivarGeneroCommand, Response<bool>>(
            new InactivarGeneroCommand
            {
                GeneroId = request.Data,
                TrackingId = request.TrackingId
            });

        #endregion
    }
}
