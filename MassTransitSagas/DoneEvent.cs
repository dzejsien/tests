using System;

namespace MassTransitSagas
{
    public class DoneEvent
    {
        public Guid? CorrelationId { get; set; }
        public int Id { get; set; }
    }
}