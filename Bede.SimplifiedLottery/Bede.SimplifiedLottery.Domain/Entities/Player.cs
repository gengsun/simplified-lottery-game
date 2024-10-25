namespace Bede.SimplifiedLottery.Domain.Entities
{
    public abstract class Player
    {
        public int Id { get; init; }

        private string? _name;
        public string Name
        {
            get => string.IsNullOrWhiteSpace(_name) ? $"Player {Id}" : _name;
            init => _name = value;
        }
    }

    public class UserPlayer : Player { }

    public class CpuPlayer : Player { }
}
