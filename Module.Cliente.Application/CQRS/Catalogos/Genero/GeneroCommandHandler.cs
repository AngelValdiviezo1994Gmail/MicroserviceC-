using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using Common.Infrastructure.Helpers;
using MediatR;
using Module.Cliente.Domain.CQRS.Command;
using Module.Cliente.Domain.Entities;

namespace Module.Cliente.Application.CQRS.CommandHandler
{
    public class GeneroCommandHandler : IRequestHandler<RegistrarGeneroCommand, Response<Genero>>
                                            , IRequestHandler<ModificarGeneroCommand, Response<Genero>>
                                            , IRequestHandler<InactivarGeneroCommand, Response<bool>>
    {
        #region Propiedades
        private readonly IGenericRepository<Genero> _iGeneroGenricRepository;
        private readonly IExecutionOrchestrator _executor;

        #endregion

        #region Constructor
        public GeneroCommandHandler(
            IGenericRepository<Genero> iSnieGenericRepository,
            IExecutionOrchestrator executor)
        => (_iGeneroGenricRepository,
            _executor) =
            (
            iSnieGenericRepository,
            executor
          );
        #endregion

        #region Handler
        public Task<Response<Genero>> Handle(RegistrarGeneroCommand request, CancellationToken cancellationToken)
        => _executor.TryCatchTransactionalAsync(
        async () =>
        {
            if (request == null) return ResponseResultHelper.RespuestaFail<Genero>(request.TrackingId, 100, "no se pudo");

            var snie = await _iGeneroGenricRepository.SelectFisrOrDefault(x =>
              (x.GenId == request.GenId || x.GesDescripcion.Trim().ToUpper() == request.GesDescripcion.Trim().ToUpper())
              && x.GenActivo == true
            );

            if (snie != null) return ResponseResultHelper.RespuestaFail<Genero>(request.TrackingId, 200, "La definición de Género ya se encuentra creado en el sistema");

            Genero snieNew = _executor.Mapper.Map<Genero>(request);

            Genero snieCreated = await _iGeneroGenricRepository.Insert(snieNew);

            if (snieCreated == null) return ResponseResultHelper.RespuestaFail<Genero>(request.TrackingId, 100, "no se pudo");

            return ResponseResultHelper.RespuestaSuccess<Genero>(request.TrackingId, snieCreated);
        },
        typeof(GeneroCommandHandler).FullName, request);

        public Task<Response<Genero>> Handle(ModificarGeneroCommand request, CancellationToken cancellationToken)
        => _executor.TryCatchTransactionalAsync(
        async () =>
        {
            if (request == null) return ResponseResultHelper.RespuestaFail<Genero>(request.TrackingId, 100, "no se pudo");

            var snie = await _iGeneroGenricRepository.SelectFisrOrDefault(w =>
                w.GenId == request.GenId
            );

            if (snie == null) return ResponseResultHelper.RespuestaFail<Genero>(request.TrackingId, 200, "No existe el Género");

            Genero snieEdit = _executor.Mapper.Map<Genero>(request);

            Genero GeneroUpdated = await _iGeneroGenricRepository.Update(snieEdit);

            if (GeneroUpdated == null) return ResponseResultHelper.RespuestaFail<Genero>(request.TrackingId, 200, "No se pudo editar el Género");

            return ResponseResultHelper.RespuestaSuccess<Genero>(request.TrackingId, GeneroUpdated);

        },
        typeof(GeneroCommandHandler).FullName, request);

        public Task<Response<bool>> Handle(InactivarGeneroCommand request, CancellationToken cancellationToken)
            => _executor.TryCatchTransactionalAsync(
                async () =>
                {
                    if (request == null) return ResponseResultHelper.RespuestaFail<bool>(request.TrackingId, 100, "la información enviada es incorrecta");

                    var claseGenero = await _iGeneroGenricRepository.SelectFisrOrDefault(w =>
                        w.GenId == request.GeneroId
                     );

                    if (claseGenero == null) return ResponseResultHelper.RespuestaFail<bool>(request.TrackingId, 200, "No existe el Género");

                    claseGenero.GenActivo = false;

                    Genero GeneroEdit = _executor.Mapper.Map<Genero>(claseGenero);

                    Genero grupoEtnicoUpdated = await _iGeneroGenricRepository.Update(GeneroEdit);

                    return ResponseResultHelper.RespuestaSuccess<bool>(request.TrackingId, true);


                },
                typeof(GeneroCommandHandler).FullName, request);

        #endregion

    }
}
