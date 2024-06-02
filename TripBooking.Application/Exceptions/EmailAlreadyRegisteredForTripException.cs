using System.Runtime.Serialization;

namespace TripBooking.Application.Exceptions
{

    public class EmailAlreadyRegisteredForTripException : Exception
    {
        public EmailAlreadyRegisteredForTripException()
        {
        }

        public EmailAlreadyRegisteredForTripException(string? message) : base(message)
        {
        }

        public EmailAlreadyRegisteredForTripException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EmailAlreadyRegisteredForTripException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
