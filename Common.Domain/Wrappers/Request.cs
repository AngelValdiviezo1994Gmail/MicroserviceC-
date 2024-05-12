using Common.Domain.Interfaces;

namespace Common.Domain.Wrappers
{
    public class Request<T> : ITracking
    {
        public T Data { get; set; }

        public Guid TrackingId { get; set; }
        public int? EscId { get; set; }

        public Request()
        {
            this.TrackingId = new Guid();
        }

        public Request(T data)
        {
            this.TrackingId = new Guid();
            this.Data = data;
        }
    }
}
