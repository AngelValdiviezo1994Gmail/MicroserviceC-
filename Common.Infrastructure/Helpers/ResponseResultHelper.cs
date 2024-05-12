using Common.Domain.Wrappers;

namespace Common.Infrastructure.Helpers
{
    public static class ResponseResultHelper
    {
        public static Response<T> RespuestaFail<T>(Guid trackingId, int errorCode, string errorMsg)
        {
            return new Response<T>
            {
                Succeeded = false,
                Error = errorMsg,
                ErrorCode = errorCode,
                TrackingId = trackingId,
            };
        }
        public static Response<T> RespuestaSuccess<T>(Guid trackingId, T entityT)
        {
            return new Response<T>
            {
                Succeeded = true,
                Error = string.Empty,
                ErrorCode = 0,
                TrackingId = trackingId,
                Data = entityT
            };
        }

    }
}
