using System;

namespace MassTransitSagas
{
    public class CalculateEvent
    {
        public Guid? CorrelationId { get; set; }
        public int Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(CorrelationId)}: {CorrelationId}, {nameof(Id)}: {Id}";
        }
    }
}