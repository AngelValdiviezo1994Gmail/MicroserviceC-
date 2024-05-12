using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using Common.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Module.Cliente.Domain.CQRS.Query;
using Module.Cliente.Domain.Entities;

namespace Module.Cliente.Application.CQRS.QueryHandler
{
    public class GeneroQueryHandler : IRequestHandler<GetGeneros, ResponsePaged<List<Genero>>>                                    
    {
        #region Propiedades
        private readonly IExecutionOrchestrator _executor;
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;
        private readonly IGenericRepository<Genero> _iSisbenGenricRepository;
        #endregion

        #region Constructor
        public GeneroQueryHandler(
            IGenericRepository<Genero> iSnieGenericRepository,
            IDbContextFactory<ApplicationContext> contextFactory,
            IExecutionOrchestrator executor
            )
        => (_iSisbenGenricRepository,
            _contextFactory,
            _executor
            ) =
            (iSnieGenericRepository,
             contextFactory,
             executor
            );
        #endregion

        #region Handlers
        public Task<ResponsePaged<List<Genero>>> Handle(GetGeneros request, CancellationToken cancellationToken)
        => _executor.TryCatchTransactionalAsync(
        async () =>
        {
            var responseList = await _iSisbenGenricRepository.
            SelectByPaged(x => x.GenId == x.GenId && x.GenActivo == true, request.Pagina, request.Cantidad, request.Orden);

            return new ResponsePaged<List<Genero>>
            {
                Succeeded = true,
                Error = string.Empty,
                ErrorCode = 0,
                TrackingId = request.TrackingId,
                Data = responseList.Data,
                Cantidad = responseList.Cantidad,
                Pagina = responseList.Pagina,
                TotalElementos = responseList.TotalElementos,
                TotalPaginas = responseList.TotalPaginas,
                Filtros = responseList.Filtros,
                Orden = responseList.Orden
            };
        },
        typeof(GeneroQueryHandler).FullName, request);

        #endregion
    }
}
