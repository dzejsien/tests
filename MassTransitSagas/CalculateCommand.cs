namespace MassTransitSagas
{
    public class CalculateCommand
    {
        public int Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}";
        }
    }
}