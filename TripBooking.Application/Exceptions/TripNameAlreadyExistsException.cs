using System.Runtime.Serialization;

namespace TripBooking.Application.Exceptions
{
    public class TripNameAlreadyExistsException : Exception
    {
        public TripNameAlreadyExistsException()
        {
        }

        public TripNameAlreadyExistsException(string? message) : base(message)
        {
        }

        public TripNameAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected TripNameAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
