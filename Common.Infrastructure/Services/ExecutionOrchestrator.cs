using AutoMapper;
using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Common.Infrastructure.Services
{
    public class ExecutionOrchestrator : IExecutionOrchestrator
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<ExecutionOrchestrator> _logger;

        public ILogger Logger => _logger;
        public IMapper Mapper => _mapper;
        public IMediator Mediator => _mediator;

        public ExecutionOrchestrator(IMapper mapper, IMediator mediator)
            => (_mapper, _mediator) = (mapper, mediator);

        public async Task<Response<TResponse>> TryCatchTransactionalAsync<TRequest, TResponse>
           (Func<Task<Response<TResponse>>> func, string method, TRequest request, Func<Exception, Response<TResponse>, Response<TResponse>> handleError = null, string errorMessage = null)
           where TRequest : ITracking
        {
            var response = Activator.CreateInstance<Response<TResponse>>();
            try
            {
                response.TrackingId = request?.TrackingId ?? Guid.Empty;
                var error = false;
                Exception exception = null;
                try
                {
                    //_logger.LogInformation(
                    //    $"Begin TryCatch execution for {method} with request: " + "{@request}, IdTransaction: {@IdTransaction}",
                    //    request, request?.TrackingId ?? Guid.Empty
                    //);
                    response = await func();
                }
                //catch (ValidationRequestException e)
                //{
                //    error = true;
                //    exception = new(e.Error, e);
                //    response.Error = e.Error;
                //    response.ErrorCode = (int)ResponseErrorCode.InvalidParameter;
                //    LogWarning(exception, request, response, method, errorMessage);
                //}
                catch (Exception e)
                {
                    error = true;
                    exception = e;
                    response.Succeeded = false;
                    response.Error = "We're sorry, something went wrong";
                    //response.ErrorCode = (int)ResponseErrorCode.InternalError;
                    response = handleError?.Invoke(exception, response) ?? response;
                    //LogError(exception, request, response, method, errorMessage);
                    System.Console.WriteLine(e);
                    System.Console.WriteLine(e.Message);
                }
                return response;
            }
            catch (Exception exception)
            {
                response.Error = "We're sorry, something went wrong";
                //response.ErrorCode = (int)ResponseErrorCode.InternalError;
                response = handleError?.Invoke(exception, response) ?? response;
                //LogError(exception, method, request, response, errorMessage);
                return response;
            }
            finally
            {
                //_logger.LogInformation(
                //    $"End TryCatch execution for {method} with reponse: " + "{@response}, IdTransaction: {@IdTransaction}",
                //    response, request?.TrackingId ?? Guid.Empty
                //);
            }
        }

        public async Task<TResponse> ProcessCommandRequest<TCommand, TResponse>(TCommand request) where TCommand : IRequest<TResponse>, ITracking
        {
            //_logger.LogDebug("MediatR ExecutorOrchestrator gRPC Server Method: {@Method}, Request: {@Request}, IdTransaction: {@IdTransaction}", context.Method, request, request?.IdTransaction ?? Guid.Empty);
            var response = await _mediator.Send(request);
            //_logger.LogDebug("MediatR ExecutorOrchestrator gRPC Server Method: {@Method}, Response: {@Response}, IdTransaction: {@IdTransaction}", context.Method, response, request?.IdTransaction ?? Guid.Empty);
            return response;
        }

    }
}
