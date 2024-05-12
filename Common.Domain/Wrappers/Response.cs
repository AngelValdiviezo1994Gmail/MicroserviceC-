using Common.Domain.Interfaces;

namespace Common.Domain.Wrappers
{
    public class Response<T> : ITracking
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Error { get; set; }
        public int ErrorCode { get; set; }
        public Guid TrackingId { get; set; }
    }
}
