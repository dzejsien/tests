using System;
using Automatonymous;
using MassTransit;
using Serilog;

namespace MassTransitSagas
{
    public class CalculationStateMachine : MassTransitStateMachine<CalculationInstance>
    {
        private readonly ILogger _log = Log.Logger.ForContext<CalculationStateMachine>();

        public Uri CalculationQueue { get; set; }

        public State Pending { get; set; }
        public State Waiting { get; set; }

        public Event<CalculateEvent> ToCalculateEvent { get; set; }
        public Event<DoneEvent> DoneEvent { get; set; }

        public CalculationStateMachine()
        {
            InitQueues();
            InitStates();
            InitEvents();

            InitWorkflow();

            SetCompletedWhenFinalized();
        }

        private void InitQueues()
        {
            CalculationQueue = new Uri(new Uri("//guest:guest@localhost/"), "ForTesting.CalculationQueue");
        }

        private void InitStates()
        {
            InstanceState(x => x.CurrentState);

            State(() => Pending);
            State(() => Waiting);
        }

        private void InitEvents()
        {
            Event(() => ToCalculateEvent, x => x.CorrelateBy((instance, ctx) => instance.Id == ctx.Message.Id).SelectId(ctx => Guid.NewGuid()));
            Event(() => DoneEvent, x => x.CorrelateBy(i => (int?)i.Id, ctx => ctx.Message.Id).OnMissingInstance(ctx => ctx.Discard()));
        }

        private void InitWorkflow()
        {
            Initially(
                When(ToCalculateEvent)
                    .Then(InitSagaInstance)
                    .Send(CalculationQueue, CreateCalculationCommand)
                    .TransitionTo(Pending));

            BeforeEnter(Pending, x => x.Then(ctx =>
            {
                _log.Information($"Before enter: {ctx.ToString()}");
            }));

            During(Pending,
                When(ToCalculateEvent)
                    .Then(ctx =>
                    {
                        _log.Information(
                            $"Skip - instance: {ctx.Instance.ToString()}, data: {ctx.Data.ToString()}");
                    })
                    .Finalize(),
                When(DoneEvent)
                    .Then(ctx =>
                    {
                        _log.Information(
                            $"Done - instance: {ctx.Instance.ToString()}, data: {ctx.Data.ToString()}");
                    })
                    .Finalize());
        }

        private static CalculateCommand CreateCalculationCommand<TData>(ConsumeEventContext<CalculationInstance, TData> context)
            where TData : class
        {
            return new CalculateCommand { Id = context.Instance.Id };
        }

        private void InitSagaInstance(BehaviorContext<CalculationInstance, CalculateEvent> context)
        {
            context.Instance.Id = context.Data.Id;
            _log.Information("--- SAGA: created: {0}", context.Instance.CorrelationId);
        }
    }
}