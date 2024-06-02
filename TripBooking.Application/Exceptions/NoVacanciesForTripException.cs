using System.Runtime.Serialization;

namespace TripBooking.Application.Exceptions
{
    public class NoVacanciesForTripException : Exception
    {
        public NoVacanciesForTripException()
        {
        }

        public NoVacanciesForTripException(string? message) : base(message)
        {
        }

        public NoVacanciesForTripException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NoVacanciesForTripException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
