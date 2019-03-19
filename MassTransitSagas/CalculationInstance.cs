using System;
using Automatonymous;
using MassTransit.MongoDbIntegration.Saga;

namespace MassTransitSagas
{
    public class CalculationInstance : SagaStateMachineInstance, IVersionedSaga
    {
        public Guid CorrelationId { get; set; }
        public int Version { get; set; }
        public string CurrentState { get; set; }

        public int Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(CorrelationId)}: {CorrelationId}, {nameof(Version)}: {Version}, {nameof(CurrentState)}: {CurrentState}, {nameof(Id)}: {Id}";
        }
    }
}