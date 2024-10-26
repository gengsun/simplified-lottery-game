namespace Bede.SimplifiedLottery.Domain.Entities
{
    public abstract class Player : BaseEntity
    {
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
