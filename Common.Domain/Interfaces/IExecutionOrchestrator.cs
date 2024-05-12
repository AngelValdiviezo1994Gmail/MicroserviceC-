using AutoMapper;
using Common.Domain.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.Domain.Interfaces
{
    public interface IExecutionOrchestrator
    {
        IMapper Mapper { get; }
        IMediator Mediator { get; }
        ILogger Logger { get; }

        Task<Response<TResponse>> TryCatchTransactionalAsync<TRequest, TResponse>
            (Func<Task<Response<TResponse>>> func,
                                    string method,
                                    TRequest request,
                                    Func<Exception, Response<TResponse>, Response<TResponse>> handleError = null,
                                    string errorMessage = null)
                                where TRequest : ITracking;

        Task<ResponsePaged<TResponse>> TryCatchTransactionalAsync<TRequest, TResponse>
            (Func<Task<ResponsePaged<TResponse>>> func,
            string method,
            TRequest request,
            Func<Exception, ResponsePaged<TResponse>, ResponsePaged<TResponse>> handleError = null,
            string errorMessage = null)
          where TRequest : ITracking;

        Task<TResponse> ProcessCommandRequest<TCommand, TResponse>(TCommand request) where TCommand : IRequest<TResponse>, ITracking;
    }
}
